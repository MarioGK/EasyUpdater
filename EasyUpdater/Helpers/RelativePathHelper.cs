using System;
using System.IO;
using System.Text;

namespace EasyUpdater.Helpers
{
    public static class RelativePathHelper
    {
        public static string RelativePath(this string absPath, string relTo)
        {
            var absDirs = absPath.Split('\\');
            var relDirs = relTo.Split('\\');
            // Get the shortest of the two paths 
            var len = absDirs.Length < relDirs.Length ? absDirs.Length : relDirs.Length;
            // Use to determine where in the loop we exited 
            var lastCommonRoot = -1; int index;
            // Find common root 
            for (index = 0; index < len; index++)
            {
                if (absDirs[index] == relDirs[index])
                {
                    lastCommonRoot = index;
                }
                else
                {
                    break;
                }
            }
            // If we didn't find a common prefix then throw 
            if (lastCommonRoot == -1)
            {
                throw new ArgumentException("Paths do not have a common base");
            }
            // Build up the relative path 
            var relativePath = new StringBuilder();
            // Add on the .. 
            for (index = lastCommonRoot + 1; index < absDirs.Length; index++)
            {
                if (absDirs[index].Length > 0)
                {
                    relativePath.Append("..\\");
                }
            }
            // Add on the folders 
            for (index = lastCommonRoot + 1; index < relDirs.Length - 1; index++)
            {
                relativePath.Append(relDirs[index] + "\\");
            }
            relativePath.Append(relDirs[^1]);
            return relativePath.ToString();
        }
    }
}