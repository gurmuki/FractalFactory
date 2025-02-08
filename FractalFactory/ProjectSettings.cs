
using FractalFactory.Common;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.IO;

namespace FractalFactory
{
    public class ProjectSettings
    {
        private string exePath = string.Empty;
        private string exeName = string.Empty;

        //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        // These data persist in the Windows registry.
        //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

        [JsonIgnore]
        public string databaseFolder { get; private set; } = string.Empty;

        [JsonIgnore]
        public string imageViewer { get; set; } = string.Empty;

        [JsonIgnore]
        public string movieGenerator { get; set; } = string.Empty;

        [JsonIgnore]
        public string movieViewer { get; set; } = string.Empty;

        //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

        // Data pertaining to fractal generation.
        public string method { get; set; } = string.Empty;
        public string numer { get; set; } = string.Empty;
        public string denom { get; set; } = string.Empty;
        public string xmin { get; set; } = string.Empty;
        public string ymin { get; set; } = string.Empty;
        public string xmax { get; set; } = string.Empty;
        public string ymax { get; set; } = string.Empty;

        // Data controlling the image presentation.
        public bool reduced { get; set; } = true;
        public bool preview { get; set; } = true;
        public int orientation { get; set; } = 0;

        // Data affecting fractal algorithm.
        public bool multi { get; set; } = true;
        public bool parallel { get; set; } = true;
        public bool limit { get; set; } = true;

        // Data affecting interpolation and value precision.
        public string precision { get; set; } = string.Empty;
        public string divs { get; set; } = string.Empty;

        // The location to use when projPath is empty. 
        public string defProjFolder { get; set; } = string.Empty;

        // The location where project .txt file was read/written.
        public string projPath { get; set; } = string.Empty;

        // The location to use when movFolder is empty. 
        public string defMovFolder { get; set; } = string.Empty;

        // Data pertaining to project movie generation.
        public string movieFolder { get; set; } = string.Empty;
        public string movieName { get; set; } = string.Empty;
        public string movieWater { get; set; } = string.Empty;
        public int movieRate { get; set; } = 12;

        /// <summary>Set the location of the fractal database.</summary>
        /// <remarks>This data is stored the Windows registry because it needs to be persistent.</remarks>
        public void DatabaseFolder(string folder)
        {
            RegistryStringSave("database", folder);
            databaseFolder = folder;
        }

        /// <summary>Saves paths to external files in the Windows registry.</summary>
        public void ExternalPathsSave()
        {
            RegistryStringSave("imageViewer", imageViewer);
            RegistryStringSave("movieGenerator", movieGenerator);
            RegistryStringSave("movieViewer", movieViewer);
        }

        public static ProjectSettings DefaultSettings()
        {
            ProjectSettings defaultSettings = new ProjectSettings();

            defaultSettings.defProjFolder = DefaultStorageProjectFolder();
            defaultSettings.defMovFolder = defaultSettings.defProjFolder;

            defaultSettings.projPath = string.Empty;
            defaultSettings.movieFolder = string.Empty;

            defaultSettings.method = OptionsDialog.NEWTON2;

            defaultSettings.DefaultParametersSet();

            defaultSettings.reduced = true;
            defaultSettings.preview = false;
            defaultSettings.multi = false;
            defaultSettings.orientation = 0;

            defaultSettings.parallel = true;
            defaultSettings.limit = true;

            defaultSettings.precision = "8";
            defaultSettings.divs = "5";

            return defaultSettings;
        }

        public void DefaultParametersSet()
        {
            numer = "x^3 - 1";
            denom = "3*x^2";

            xmin = "-2.5";
            ymin = "-2.1";
            xmax = "2.5";
            ymax = "2.1";
        }

        public ProjectSettings()
        {
            exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            exeName = ProperName(System.IO.Path.GetFileNameWithoutExtension(exePath));

            databaseFolder = RegistryStringGet("database", System.IO.Path.GetDirectoryName(exePath)!);
            imageViewer = RegistryStringGet("imageViewer", string.Empty);
            movieGenerator = RegistryStringGet("movieGenerator", string.Empty);
            movieViewer = RegistryStringGet("movieViewer", string.Empty);
        }

        /// <summary>Get the settings as a json string.</summary>
        public string Serialize()
        {
            string json = JsonConvert.SerializeObject(this);
            return json;
        }

        /// <summary>Initialize a settings object using the json string.</summary>
        public void Deserialize(string json)
        {
            if (Stringy.IsEmpty(json))
                throw new InvalidOperationException("ProjectSettings.Deserialize() -- invalid json string.");

            ProjectSettings settings = JsonConvert.DeserializeObject<ProjectSettings>(json)!;

            Copy(settings);
        }

        // There's probably a more generic way of doing this.
        public void Copy(ProjectSettings settings)
        {
            method = settings.method;

            numer = settings.numer;
            denom = settings.denom;

            xmin = settings.xmin;
            ymin = settings.ymin;
            xmax = settings.xmax;
            ymax = settings.ymax;

            reduced = settings.reduced;
            preview = settings.preview;
            orientation = settings.orientation;

            multi = settings.multi;
            parallel = settings.parallel;
            limit = settings.limit;

            precision = settings.precision;
            divs = settings.divs;

            defProjFolder = settings.defProjFolder;
            projPath = settings.projPath;

            defMovFolder = settings.defMovFolder;
            movieFolder = settings.movieFolder;
            movieName = settings.movieName;
            movieWater = settings.movieWater;
            movieRate = settings.movieRate;

            imageViewer = settings.imageViewer;
            movieGenerator = settings.movieGenerator;
            movieViewer = settings.movieViewer;
        }

        /// <summary>Get the folder associated with projectPath</summary>
        public string ProjectFolder()
        {
            string folder = PathFuncs.Folder(projPath);
            return ((folder == string.Empty) ? defProjFolder : folder);
        }

        /// <summary>Get the file name associated with projectPath</summary>
        public string ProjectFilename()
        {
            return PathFuncs.FileName(projPath);
        }

        private string RegistryStringGet(string name, string defaultValue)
        {
            string value = string.Empty;

            RegistryKey? key = Registry.CurrentUser.OpenSubKey("Software", true);
            if (key == null)
                throw new InvalidOperationException("ProjectSettings.RegistryStringGet() -- can't open registry.");

            RegistryKey? subkey = key.OpenSubKey(exeName, true);
            if (subkey == null)
            {
                subkey = key.CreateSubKey(exeName);
                subkey.OpenSubKey(exeName, true);
            }

            value = (string)subkey.GetValue(name)!;
            if (value == null)
            {
                subkey.SetValue(name, defaultValue);
                value = defaultValue;
            }

            subkey.Close();
            key.Close();

            return value;
        }

        private void RegistryStringSave(string name, string value)
        {
            RegistryKey? key = Registry.CurrentUser.OpenSubKey("Software", true);
            if (key == null)
                throw new InvalidOperationException("ProjectSettings.RegistryStringSave() -- can't open registry.");

            RegistryKey? subkey = key.OpenSubKey(exeName, true);
            if (subkey == null)
            {
                subkey = key.CreateSubKey(exeName);
                subkey.OpenSubKey(exeName, true);
            }

            subkey.SetValue(name, value);

            subkey.Close();
            key.Close();
        }

        private static string DefaultStorageProjectFolder()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        private static string ProperName(string path)
        {
            string tmp = path;

            while (true)
            {
                int indx = tmp.LastIndexOf('.');
                if (indx < 0)
                    return tmp;

                // Guard against paths like C:\foo.xyz\file
                int jndx = tmp.LastIndexOf('\\');
                if (jndx > indx)
                    return tmp;

                tmp = tmp.Substring(0, indx);
            }
        }
    }
}
