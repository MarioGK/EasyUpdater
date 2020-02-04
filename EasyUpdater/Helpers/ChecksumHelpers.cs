using System;
using System.IO;

namespace EasyUpdater.Helpers
{
    public static class ChecksumHelpers
    {
        public static string CalculateChecksum(this string filename)
        {
            using var stream = File.OpenRead(filename);
            var hash = EasyUpdater.HashAlgorithm.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}