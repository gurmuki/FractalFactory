using FractalFactory.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;


namespace FractalFactory.Database
{
    /// <summary>
    ///     <para>
    ///     FractalDB is an API providing operations on its underlying SQLite database.
    ///     The database itself ("fractal.db") is created in the runtime directory and
    ///     provides a persistent/portable mechanism for managing fractal projects.
    ///     </para>
    ///     <para>
    ///     Conceptually, the database has two segments: projects and a workspace. A Project
    ///     is never directly operated upon by an application. Instead, all all operations
    ///     are performed on the Workspace. The Workspace can be populated with Project
    ///     contents and the Workspace can be written to a Project.
    ///     </para>
    /// </summary>
    public class FractalDB
    {
        const string WORKSPACE = "_WORKSPACE_";
        const string CLONE = "_CLONE_";

        private const string DIRECTORY_TABLE_CREATE = "CREATE TABLE IF NOT EXISTS Directory (" +
            "ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
            "Name VARCHAR(255) NOT NULL UNIQUE);";

        private const string SETTINGS_TABLE_CREATE = "CREATE TABLE IF NOT EXISTS Settings (" +
            "ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
            "DirectoryID INT," +
            "Json VARCHAR(255) NOT NULL);";

        private const string PROJECT_TABLE_CREATE = "CREATE TABLE IF NOT EXISTS {0} (" +
            "ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
            "DirectoryID INT," +
            "RowNum INT," +
            "Text VARCHAR(255) NOT NULL," +
            "Valid INT," +
            "Image BLOB);";  // NOT NULL);

        private SQLiteConnection? connection;
        private SQLiteTransaction? transaction;

        private const int BOTTOM = -1;
        private int depth;

        // transactionEnabled was introduced to work around an SQL logic exception
        // thrown while processing the statement "pragma synchronous = normal"
        private bool transactionEnabled = true;

        // 'imageSize' is used simply to create a small "placeholder" blob. Historically,
        // the placeholder was quite large, representing the size of an uncompressed bitmap.
        // 'imageSize' is likely altogether unnecessary but I don't want to deal with the
        // NULL blob issue. Related, a blob will have a size of 8 only when its database
        // 'Valid' field is zero. As such, the 'Valid' field is (essentially) redundant
        // and could(?) be eliminated.
        private string imageSize = "zeroblob(8)";

        byte[] compressed = { };

        //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

        public FractalDB()
        {
            connection = null;
            transaction = null;
            WorkspaceID = -1;
        }

        public bool IsNewDatabase { get; private set; }

        public bool DbInit(string dbPath)
        {
            bool success = false;

            connection = new SQLiteConnection($"Data Source={dbPath}; Version=3; New=True; Compress=True;");
            try
            {
                depth = BOTTOM;

                connection.Open();

                // Performance optimization.
                //   From https://phiresky.github.io/blog/2020/sqlite-performance-tuning/
                transactionEnabled = false;
                NonQueryExecute("pragma journal_mode = WAL");
                NonQueryExecute("pragma synchronous = normal");
                transactionEnabled = true;
                // NonQueryExecute("pragma mmap_size = 30000000000");
                // NonQueryExecute("pragma page_size = 32768");

                if (TablesInit())
                {
                    long id = ProjectID(WORKSPACE);

                    IsNewDatabase = (id < 1);
                    if (IsNewDatabase)
                        id = ProjectNew(WORKSPACE);

                    WorkspaceID = id;
                    success = (id > 0);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            IsDirty = false;

            return success;
        }

        public void Close()
        {
            if (connection != null)
            {
                transactionEnabled = false;

                try
                {
                    // To eliminate the -shm and -wal files that linger ....
                    //   From https://stackoverflow.com/questions/57012440/remove-temporary-room-sqlite-files
                    NonQueryExecute("PRAGMA wal_checkpoint(TRUNCATE)");
                    NonQueryExecute("PRAGMA journal_mode=DELETE;");
                }
#pragma warning disable CS0168, CS0219
                catch (Exception ex)
                {
                    int breakpoint = 0;
                }
#pragma warning restore CS0168, CS0219

                connection.Close();
                connection = null;

                IsDirty = false;
            }
        }

        /// <summary>Sets/Get the expected size (number of bytes) of retrieved bitmaps.</summary>
        /// <remarks>
        /// Used by FractalDB.WorkspaceImageAt(), BitmapSize is a CRITICAL optimization
        /// whose purpose is the reduce garbage collection.
        /// </remarks>
        public int BitmapSize
        {
            get { return compressed.Length; }
            set { compressed = new byte[value]; }
        }

        /// <summary>A flag indicating whether the database has been modified.</summary>
        public bool IsDirty { get; set; }

        /// <summary>The reserved ID representing the workspace table.</summary>
        public long WorkspaceID { get; private set; }

        /// <summary>Returns true if the given ID is that of the workspace table.</summary>
        public bool IsWorkspaceID(long projectID)
        {
            return (projectID == WorkspaceID);
        }

        /// <summary>Get the count of projects.</summary>
        /// <returns>The number of projects in the database.</returns>
        public int ProjectsCount()
        {
            int recordCount = 0;

            using (var reader = QueryExecute($"SELECT COUNT(*) FROM Directory WHERE (Name!='{WORKSPACE}')"))
            {
                if (reader!.Read())
                    recordCount = Int32.Parse(reader["COUNT(*)"].ToString()!);
            }

            return recordCount;
        }

        /// <summary>Creates a new project.</summary>
        /// <param name="projectName">the name of the project to create</param>
        /// <returns>the id associated with the project name</returns>
        public long ProjectNew(string projectName)
        {
            long id = -1;

            try
            {
                if (ActionExecute($"INSERT INTO Directory (Name) VALUES ('{projectName}')"))
                {
                    id = LastRowID("Directory");
                    IsDirty = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return id;
        }

        /// <summary>Get the name of a project.</summary>
        /// <param name="projectId">the id representing the project of interest</param>
        /// <returns>(empty) failure / (otherwise) the project name</returns>
        public string ProjectName(long projectId)
        {
            string projectName = string.Empty;

            if (IsWorkspaceID(projectId) || (projectId <= 1))
                return projectName;

            try
            {
                using (var reader = QueryExecute($"SELECT Name FROM Directory WHERE (ID={projectId})"))
                {
                    if (reader!.Read())
                        projectName = reader["Name"].ToString()!;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return projectName;
        }

        /// <summary>Gets a list for Project names from the database.</summary>
        /// <returns>a list of project names</returns>
        public List<string> ProjectNames()
        {
            List<string> projectNames = new List<string>();

            try
            {
                using (var reader = QueryExecute($"SELECT Name FROM Directory WHERE (name!='{WORKSPACE}') ORDER BY Name ASC"))
                {
                    while (reader!.Read())
                    {
                        string name = reader["Name"].ToString()!;
                        if (name != WORKSPACE)
                            projectNames.Add(name);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return projectNames;
        }

        /// <summary>Get the id representing the named project.</summary>
        /// <param name="projectName">the targeted project name</param>
        /// <returns>(>0) the project id / (otherwise) failure</returns>
        public long ProjectID(string projectName)
        {
            long projectID = -1;

            try
            {
                using (var reader = QueryExecute($"SELECT ID FROM Directory WHERE (Name='{projectName}')"))
                {
                    if (reader!.Read())
                        projectID = (long)reader["ID"];
                }

                if (projectID > 0)
                {
                    // But does the associated named table exist? It won't if
                    // we just created a new database, i.e. via File/Options.
                    if (!TableExists(projectName))
                        projectID = -1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return projectID;
        }

        /// <summary> Deletes records related to a project. </summary>
        public bool ProjectDelete(long projectID)
        {
            string projectName = ProjectName(projectID);
            if (!Stringy.IsEmpty(projectName))
            {
                TransactionBegin();

                NonQueryExecute($"DROP TABLE IF EXISTS {projectName}");
                NonQueryExecute($"DELETE FROM Settings WHERE (DirectoryID = {projectID})");
                NonQueryExecute($"DELETE FROM Directory WHERE (ID = {projectID})");

                TransactionEnd();

                return true;
            }

            return false;
        }

        /// <summary>Rename an existing project.</summary>
        /// <param name="existingProjectName">the name of an existing project</param>
        /// <param name="newProjectName">its new name</param>
        /// <returns>(true) success / (false) failure</returns>
        public bool ProjectRename(string existingProjectName, string newProjectName)
        {
            bool success = false;

            long existingProjectID = ProjectID(existingProjectName);
            long newProjectID = ProjectID(newProjectName);

            if (existingProjectID <= 0)
                throw new InvalidOperationException("The existing project doesn't exist!");

            if (newProjectID > 0)
                throw new InvalidOperationException("The new project must first be deleted!");

            TransactionBegin();

            if (!NonQueryExecute($"ALTER TABLE {existingProjectName} RENAME TO {newProjectName}"))
                throw new Exception($"Failed to rename table {existingProjectName} to {newProjectName}.");

            success = ActionExecute($"UPDATE Directory SET Name='{newProjectName}' WHERE (ID={existingProjectID})");

            TransactionEnd();

            return success;
        }

        /// <summary>Saves the settings used by a project.</summary>
        /// <param name="projectID">the project id</param>
        /// <param name="settings">the settings as a json string</param>
        /// <returns>(true) success / (false) failure</returns>
        public bool SettingsSave(long projectID, string settings)
        {
            bool success = false;

            try
            {
                using (var reader = QueryExecute($"SELECT * FROM Directory WHERE (ID='{projectID}')"))
                { success = reader!.HasRows; }

                if (success)
                {
                    TransactionBegin();

                    using (var reader = QueryExecute($"SELECT * FROM Settings WHERE (DirectoryID={projectID})"))
                    {
                        if (reader!.HasRows)
                            success = ActionExecute($"UPDATE Settings SET Json = '{settings}' WHERE (DirectoryID={projectID})");
                        else
                            success = ActionExecute($"INSERT INTO Settings (DirectoryID, Json) VALUES ({projectID}, '{settings}')");

                        if (success)
                            IsDirty = true;
                    }

                    TransactionEnd();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return success;
        }

        /// <summary>Gets the settings used by a project.</summary>
        /// <param name="projectID">the project id</param>
        /// <returns>(empty) failure / (otherwise) the settings as a json string</returns>
        public string SerializedSettings(long projectID)
        {
            string settings = string.Empty;

            try
            {
                using (var reader = QueryExecute($"SELECT Json FROM Settings WHERE (DirectoryID='{projectID}')"))
                {
                    if (reader!.Read())
                        settings = reader["Json"].ToString()!;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return settings;
        }

        /// <summary>Creates an empty workspace.</summary>
        public void WorkspaceClear()
        {
            TransactionBegin();

            string sqlStatment = $"DROP TABLE IF EXISTS {WORKSPACE}";
            NonQueryExecute(sqlStatment);

            sqlStatment = string.Format(PROJECT_TABLE_CREATE, WORKSPACE);
            NonQueryExecute(sqlStatment);

            TransactionEnd();

            IsDirty = false;
        }

        /// <summary>Populate the workspace using the data from the targeted project.</summary>
        /// <param name="projectID">the id of the referenced project</param>
        /// <returns>(>0) the id of the workspace project / (otherwise) failure</returns>
        public long WorkspacePopulate(long projectID)
        {
            string projectName = ProjectName(projectID);

            if (Stringy.IsEmpty(projectName))
                throw new Exception($"No project exists have an id of {projectID}");

            if (!TableExists(projectName))
                throw new Exception($"No table exists have the name {projectName}");

            if (TableExists(CLONE))
                throw new Exception($"A {CLONE} table exists (indicating an earlier error).");

            NonQueryExecute($"DROP TABLE {WORKSPACE}");

            // Clone the project into the workspace.
            if (!NonQueryExecute($"CREATE TABLE {WORKSPACE} AS SELECT * FROM '{projectName}'"))
                throw new Exception($"Failed to clone the project {projectName} into {WORKSPACE}.");

            return projectID;
        }

        /// <summary>Allows the app to update the grid control while loading a file.</summary>
        public delegate void DisplayFunc(int indx, string str);

        public void WorkspacePopulate(List<string> initialContent, DisplayFunc f)
        {
            TransactionBegin();

            WorkspaceClear();

            int rowNumber = 0;
            foreach (string str in initialContent)
            {
                string statement = Preprocess(str);
                if (statement.Length < 1)
                    continue;

                WorkspaceRecordNew(statement, rowNumber);
                if (f != null)
                    f(rowNumber, str);

                ++rowNumber;
            }

            TransactionEnd();

            IsDirty = (rowNumber > 0);
        }

        /// <summary>Support legacy project files.</summary>
        public string Preprocess(string statement)
        {
            string trimmed = Stringy.Trim(statement);
            if (trimmed.Length < 1)
                return trimmed;

            string substituedA = trimmed.Replace("1st:", Stringy.NUMERC);
            string substituedB = substituedA.Replace("2nd:", Stringy.DENOMC);

            return substituedB;
        }

        /// <summary>Save the Workspace records to the indicated Project.</summary>
        /// <param name="projectName">the name of the target project.</param>
        /// <returns>(>0) the project id of target project / (otherwise) failure.</returns>
        public long WorkspaceSaveTo(long projectID)
        {
            string projectName = ProjectName(projectID);
            if (Stringy.IsEmpty(projectName))
                throw new Exception($"No project exists having an id of {projectID}.");

            if (TableExists(CLONE))
                throw new Exception($"A {CLONE} table exists (indicating an earlier error).");

            //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Clone the workspace.
            // Whatever the reason, the SQLite statement: CREATE TABLE _CLONE_ AS SELECT * FROM _WORKSPACE_
            // failed to carry forward the schema directive: ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT
            // causing problems downstream. So now a two-step solution is used.
            //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            string sqlStatment = string.Format(PROJECT_TABLE_CREATE, CLONE);
            if (!NonQueryExecute(sqlStatment))
                throw new Exception($"Failed to create the {CLONE} table.");

            if (!NonQueryExecute($"INSERT INTO {CLONE} (ID, DirectoryID, RowNum, Text, Valid, Image) SELECT * FROM {WORKSPACE}"))
                throw new Exception($"Failed to clone {WORKSPACE}.");
            //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

            // Drop any existing table.
            if (!NonQueryExecute($"DROP TABLE IF EXISTS '{projectName}'"))
                throw new Exception($"Failed to drop the {projectName} table.");

            // Rename the cloned table.
            if (!NonQueryExecute($"ALTER TABLE {CLONE} RENAME TO '{projectName}'"))
                throw new Exception($"Failed to rename the {CLONE} table.");

            IsDirty = false;

            return projectID;
        }

        /// <summary>Saves the Workspace statements to a text file.</summary>
        /// <param name="filePath">path to text file</param>
        /// <returns>(true) success / (false) failure</returns>
        public bool WorkspaceSaveTo(string filePath)
        {
            bool success = false;

            using (var reader = QueryExecute($"SELECT Text FROM {WORKSPACE} ORDER BY RowNum ASC"))
            {
                if (reader!.StepCount > 0)
                {
                    using (StreamWriter file = new StreamWriter(filePath))
                    {
                        while (reader.Read())
                        {
                            file.WriteLine((string)reader["Text"]);
                        }
                    }
                }
            }


            return success;
        }

        /// <summary>Get the number records in the Workspace.</summary>
        /// <returns>(>0) the number records / (otherwise) no records exist (unlikely)</returns>
        public int WorkspaceRecordCount()
        {
            // Practically speaking, the Workspace table is a temporary table representing the
            // contents of an active project and so it will likely never have more than records
            // than int.MaxValue (if so, generating the images would take an extremely long time).
            int recordCount = 0;

            using (var reader = QueryExecute($"SELECT COUNT(*) FROM {WORKSPACE}"))
            {
                if (reader!.Read())
                    recordCount = Int32.Parse(reader["COUNT(*)"].ToString()!);
            }

            return recordCount;
        }

        /// <summary>Creates a new Workspace record.</summary>
        /// <param name="statement">a statement</param>
        /// <param name="rowNumber">the row number of the statement</param>
        /// <returns>(>0) the id of the record / (otherwise) failure</returns>
        public long WorkspaceRecordNew(string statement, int rowNumber)
        {
            long id = -1;

            try
            {
                if (ActionExecute($"INSERT INTO {WORKSPACE} (DirectoryID, RowNum, Text, Valid, Image) VALUES ({WorkspaceID}, {rowNumber}, '{statement}', 0, {imageSize})"))
                {
                    id = LastRowID(WORKSPACE);
                    IsDirty = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return id;
        }

        /// <summary>Updates the targeted workspace statement.</summary>
        /// <param name="rowNumber">the row number of the targeted statement</param>
        /// <param name="statement">a statement</param>
        /// <returns>{true) success / (false) failure</returns>
        public bool WorkspaceStatementUpdate(int rowNumber, string statement)
        {
            bool success = ActionExecute($"UPDATE {WORKSPACE} SET Text = '{statement}' WHERE (RowNum = {rowNumber})");
            if (success)
                IsDirty = true;

            return success;
        }

        /// <summary>Deletes the specified record from the Workspace table.</summary>
        /// <remarks>Bound calls to WorkspaceRecordDelete with TransactionBegin/TransactionEnd as necessary.</remarks>
        /// <returns>(true) success / (false) failure</returns>
        public bool WorkspaceRecordDelete(int rowNumber)
        {
            bool success = false;
            // eg. DELETE FROM mytable WHERE rowid=1;
            try
            {
                success = ActionExecute($"DELETE FROM {WORKSPACE} WHERE (RowNum = {rowNumber})");
                if (success)
                    IsDirty = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return success;
        }

        /// <summary>Get a statement from the Workspace.</summary>
        /// <param name="rowNumber">the row number of the record</param>
        /// <returns>a string statement</returns>
        public string WorkspaceStatementFetch(int rowNumber)
        {
            string statement = string.Empty;
            try
            {
                using (var reader = QueryExecute($"SELECT Text FROM {WORKSPACE} WHERE (RowNum = {rowNumber})"))
                {
                    if (reader!.Read())
                        statement = reader["Text"].ToString()!;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return statement;
        }

        /// <summary>Get all statements from the Workspace, ordered by RowNum.</summary>
        /// <returns>a list of string statements</returns>
        public List<string> WorkspaceStatements()
        {
            List<string> statements = new List<string>();

            try
            {
                using (var reader = QueryExecute($"SELECT Text FROM {WORKSPACE} ORDER BY RowNum ASC"))
                {
                    while (reader!.Read())
                        statements.Add(reader["Text"].ToString()!);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return statements;
        }

        /// <summary>Determine if the workspace has an image at the targeted record.</summary>
        /// <param name="rowNumber">the targeted record</param>
        /// <returns>(true) yes / (false) no</returns>
        public bool WorkspaceHasImageAt(int rowNumber)
        {
            if (rowNumber < 0)
                throw new InvalidOperationException("rowNumber < 0");

            // The field 'Valid' was (in theory) introduced for efficiency. Given how image blobs
            // are updated, there is always space reserved for a blob but it not know if the
            // associated image represents anything meaningful.
            using (var reader = QueryExecute($"SELECT Valid FROM {WORKSPACE} WHERE ((RowNum = {rowNumber}) AND (Valid = 1))"))
            {
                if ((reader == null) || !reader.HasRows)
                    return false;
            }

            return true;
        }

        /// <summary>Get the bitmap associated with the targeted record.</summary>
        /// <param name="rowNumber">the targeted record</param>
        /// <param name="bitmap">the space reserved for the bitmap</param>
        /// <remarks>See also ImageSize()</remarks>
        /// <returns>(true) success / (false) failure</returns>
        public bool WorkspaceImageAt(int rowNumber, byte[] bitmap)
        {
            if (bitmap.Length != compressed.Length)
                throw new InvalidOperationException("See FractalDB.BitmapSize");

            if (!WorkspaceHasImageAt(rowNumber))
                return false;

            SQLiteCommand cmd = new SQLiteCommand($"SELECT Image FROM {WORKSPACE} WHERE (RowNum = {rowNumber})", connection);
            using (SQLiteDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
            {
                reader.Read();

                // bitmap.Length will always be greater-than-or-equal to the
                // actual number of bytes written into compressed[].
                long retval = reader.GetBytes(0, 0, compressed, 0, compressed.Length);
                if (retval > 0)
                    ByteConverter.ConvertByteDecompress(compressed, bitmap);
            }

            return true;
        }

        /// <summary>Compact the database tables.</summary>
        public void Compact()
        {
            try
            {
                SQLiteCommand command = new SQLiteCommand("VACUUM", connection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>Updates the display order of all (necessary) records.</summary>
        /// <remarks>Use after insert/delete operations.</remarks>
        /// <param name="rowNumber">Where the insert/delete operation was applied.</param>
        /// <param name="count">The count of records inserted/deleted.</param>
        public void DisplayOrderUpdate(int rowNumber, int count)
        {
            if (count < 1)
                return;  // Nothing to do.

            try
            {
                using (var reader = QueryExecute($"SELECT ID FROM {WORKSPACE} WHERE (RowNum > {rowNumber})"))
                {
                    if (reader == null)
                        return;

                    TransactionBegin();

                    while (reader.Read())
                    {
                        if (!reader.HasRows)
                            break;

                        long rowID = Int64.Parse(reader["ID"].ToString()!);
                        ActionExecute($"UPDATE {WORKSPACE} SET RowNum = (RowNum + {count}) WHERE (ID = {rowID})");
                    }

                    TransactionEnd();

                    IsDirty = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>Updates the display order of all (necessary) records.</summary>
        /// <remarks>Use after insert/delete operations.</remarks>
        /// <param name="rowNumber">The row below which all records should be renumbered.</param>
        public void DisplayOrderUpdate(int baseRowNumber)
        {
            try
            {
                using (var reader = QueryExecute($"SELECT RowNum FROM {WORKSPACE} WHERE (RowNum > {baseRowNumber}) ORDER BY RowNum ASC"))
                {
                    TransactionBegin();

                    int newRowNumber = baseRowNumber + 1;
                    while (reader!.Read())
                    {
                        int oldRowNumber = (int)reader["RowNum"];
                        ActionExecute($"UPDATE {WORKSPACE} SET RowNum = {newRowNumber} WHERE (RowNum = {oldRowNumber})");
                        ++newRowNumber;
                    }

                    TransactionEnd();

                    IsDirty = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>Determine whether workspace contains images.</summary>
        /// <returns>The count of images</returns>
        public int WorkspaceImageCount()
        {
            int count = 0;

            using (var reader = QueryExecute($"SELECT COUNT(Valid) FROM {WORKSPACE} WHERE (Valid == 1)"))
            {
                if (reader!.Read())
                    count = Int32.Parse(reader["COUNT(Valid)"].ToString()!);
            }

            return count;
        }

        /// <summary>Set the status of a record to indicate it has (or not) an image.</summary>
        /// <param name="rowNumber">the targeted record</param>
        /// <param name="isValid">(true) has / (false) doesn't have</param>
        /// <remarks>See also WorkspaceHasImageAt()</remarks>
        /// <returns>{true) success / (false) failure</returns>
        public bool WorkspaceImageClear(int rowNumber)
        {
            bool success = ActionExecute($"UPDATE {WORKSPACE} SET Valid = 0, Image = {imageSize} WHERE (RowNum = {rowNumber})");
            if (success)
                IsDirty = true;

            return success;
        }

        public bool WorkspaceImageUpdate(int rowNumber, byte[] bitmap)
        {
            if (rowNumber < 0)
                return false;

            try
            {
                byte[] compressed = ByteConverter.ConvertByteCompress(bitmap);

                SQLiteCommand cmd = new SQLiteCommand($"UPDATE {WORKSPACE} SET Image=@image, Valid=1 WHERE (RowNum={rowNumber})", connection);
                cmd.Parameters.Add("@image", DbType.Binary, compressed.Length).Value = compressed;

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return true;
        }

        /// <summary>Mark the beginning of a database transaction.</summary>
        /// <remarks>
        ///     <para>
        ///     Calls to TransactionBegin()/TransactionEnd() can be nested but only
        ///     the outermost calls have any effect, allowing for greater transactional
        ///     granularity.
        ///     </para>
        ///     <para>
        ///     Internal to FractalDB, all methods resulting in database
        ///     modification are bound by calls to TransactionBegin()/TransactionEnd().
        ///     </para>
        /// </remarks>
        public void TransactionBegin()
        {
            if (depth == BOTTOM)
            {
                if (transaction != null)  // unlikely but ....
                    throw new InvalidOperationException("TransactionBegin() -- transaction is not null.");

                try
                {
                    transaction = connection!.BeginTransaction();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else if (depth < BOTTOM)
            {
                throw new InvalidOperationException("TransactionBegin() -- nesting error.");
            }
            else
            {
                // ASSUMPTION: Handling a nested call. Nothing to do.
            }

            ++depth;
        }

        /// <summary>Commit and terminate an active transaction.</summary>
        /// <returns>(true) success / (false) failure</returns>
        public void TransactionEnd()
        {
            --depth;

            if (depth == BOTTOM)
            {
                if (transaction == null)  // unlikely but ....
                    throw new InvalidOperationException("TransactionEnd() -- transaction is null.");

                try
                {
                    transaction.Commit();
                    transaction = null;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else if (depth < BOTTOM)
            {
                throw new InvalidOperationException("TransactionEnd() -- nesting error.");
            }
            else
            {
                // ASSUMPTION: Handling a nested call. Nothing to do.
            }
        }

        //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

        private bool TablesInit()
        {
            bool success = false;

            try
            {
                TransactionBegin();

                NonQueryExecute(DIRECTORY_TABLE_CREATE);
                NonQueryExecute(SETTINGS_TABLE_CREATE);

                string sqlStatment = string.Format(PROJECT_TABLE_CREATE, WORKSPACE);
                NonQueryExecute(sqlStatment);

                TransactionEnd();

                success = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return success;
        }

        private long LastRowID(string tableName)
        {
            long id = -1;

            try
            {
                SQLiteCommand command = new SQLiteCommand($"SELECT last_insert_rowid() AS ID FROM '{tableName}'", connection);
                id = (long)command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return id;
        }

        private bool TableExists(string tableName)
        {
            bool exists = false;

            using (var reader = QueryExecute($"SELECT name FROM sqlite_master WHERE (type='table') AND (name='{tableName}')"))
            {
                if (reader!.Read())
                    exists = true;
            }

            return exists;
        }

        private bool ValidUpdate(string tableName, int id, bool isValid)
        {
            int valid = (isValid ? 1 : 0);
            bool success = ActionExecute($"UPDATE {tableName} SET Valid = {valid} WHERE (ID = {id})");
            if (success)
                IsDirty = true;

            return success;
        }

        /// <summary>Use for statements that, in effect, write to the database.</summary>
        /// <param name="sqlStatement">the statement to execute</param>
        /// <returns>(true) success / (false) failure</returns>
        private bool ActionExecute(string sqlStatement)
        {
            bool success = false;

            try
            {
                SQLiteCommand command = new SQLiteCommand(sqlStatement, connection);

                TransactionBegin();
                int rowsAdded = command.ExecuteNonQuery();
                TransactionEnd();

                // 'rowsAdded' can be zero. For example, when creating a new project that
                // overwrites an existing project
                //   e.g. "INSERT INTO Workspace SELECT * FROM Project WHERE (DirectoryID = 7)"
                // the Project table may not have any records.
                success = (rowsAdded > 0);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return success;
        }

        /// <summary>Use for statements that, in effect, read from the database.</summary>
        /// <param name="sqlStatement">the statement to execute</param>
        /// <returns>(null) failure / (otherwise) a SQLiteDataReader object</returns>
        private SQLiteDataReader? QueryExecute(string sqlStatement)
        {
            try
            {
                SQLiteCommand command = new SQLiteCommand(sqlStatement, connection);
                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>Use for DELETE and pragma statements.</summary>
        /// <param name="sqlStatement">the statement to execute</param>
        /// <returns>(true) success / (false) failure</returns>
        private bool NonQueryExecute(string sqlStatement)
        {
            bool success = false;

            try
            {
                SQLiteCommand command = new SQLiteCommand(sqlStatement, connection);

                if (transactionEnabled)
                    TransactionBegin();

                command.ExecuteNonQuery();

                if (transactionEnabled)
                    TransactionEnd();

                success = true;
            }
            catch (Exception ex)
            {
                // ASSUMPTION: This will only happen when the db is locked
                // (eg. open the SQLite browser). If so, falling through simply
                // leaves a .db-shm and .db-wal file behinds.
                Debug.WriteLine($"Assertion: ExecuteNonQuery() -- {ex.Message}");
            }

            return success;
        }
    }
}
