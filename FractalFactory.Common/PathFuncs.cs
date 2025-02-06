using System.IO;

namespace FractalFactory.Common
{
    public class PathFuncs
    {
        /// <summary>Get the folder associated with projectPath</summary>
        public static string Folder(string path)
        {
            if (Stringy.IsEmpty(path))
                return string.Empty;

            if (ContainsFileName(path))
                return Path.GetDirectoryName(path)!;

            return path;
        }

        public static string FolderName(string path)
        {
            if (Stringy.IsEmpty(path))
                return string.Empty;

            string tmp = path;

            if (ContainsFileName(tmp))
                tmp = Path.GetDirectoryName(tmp)!;

            int indx = tmp.LastIndexOf('\\');
            return ((indx < 0) ? string.Empty : tmp.Substring(indx + 1));
        }

        /// <summary>Get the file name associated with projectPath</summary>
        public static string FileName(string path)
        {
            if (Stringy.IsEmpty(path))
                return string.Empty;

            if (ContainsFileName(path))
                return Path.GetFileName(path);

            return string.Empty;
        }

        public static bool ContainsFileName(string path)
        {
            string ext = Path.GetExtension(path);
            return !Stringy.IsEmpty(ext);
        }
    }
}
