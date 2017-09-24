using System.IO;
using System.Reflection;
using System.Text;

namespace Framework.Infrastructure.Utils
{
    public class FileUtils
    {
        public static ulong GetDirectorySize(string dir, string filter = "*.*", bool ignoreHiddenFiles = false)
        {
            var filenameList = Directory.GetFiles(dir, filter);
            ulong size = 0;
            foreach (var filename in filenameList)
            {
                var info = new FileInfo(filename);
                if (ignoreHiddenFiles)
                {
                    //ignore the hidden files
                    if ((info.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                        continue;
                }

                size += (ulong)info.Length;
            }

            return size;
        }

        public static string GetFileDirectory(string exeName)
        {
            try
            {
                var fInfo = new FileInfo(exeName);
                return fInfo.DirectoryName;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetApplicationExeDirectory()
        {
            try
            {
                var fi = new FileInfo(Assembly.GetEntryAssembly().Location);
                return fi.DirectoryName;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string Combine(string path1, string path2, params string[] paramstrs)
        {
            return CombineInternal(Path.DirectorySeparatorChar, path1, path2, paramstrs);
        }

        private static string CombineInternal(char slash, string path1, string path2, params string[] paramstrs)
        {
            return string.Format("{0}{1}{2}{3}", path1.RemoveLastChar(slash), slash, path2.RemoveFirstChar(slash), (paramstrs == null || paramstrs.Length == 0) ? string.Empty : PathString(paramstrs, slash));
        }

        private static string PathString(string[] paramstrs, char slash)
        {
            var sb = new StringBuilder();
            foreach (var item in paramstrs)
            {
                sb.Append(StringUtils.RemoveLastCharAndAddFirstChar(item, slash));
            }

            return sb.ToString();
        }
    }
}
