using System;
using System.Diagnostics;
using System.IO;

namespace doob.Who.Helper
{
    public class PathHelper
    {

        public static string ContentPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName);


        public static string GetFullPath(string path, string basePath = null)
        {
            if (String.IsNullOrWhiteSpace(basePath))
            {
                basePath = ContentPath;
            }
            var p = Path.GetFullPath(Path.Combine(basePath, path));
            return p;
        }
    }
}
