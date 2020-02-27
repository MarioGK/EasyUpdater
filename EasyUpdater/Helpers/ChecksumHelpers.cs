using System;
using System.IO;
using EasyUpdater.Crypto;

namespace EasyUpdater.Helpers
{
    public static class ChecksumHelpers
    {
        public static Crc32 HashAlgorithm { get; set; } = new Crc32();
        
        public static string CalculateChecksum(this string filename)
        {
            using var stream = File.OpenRead(filename);
            var hash = HashAlgorithm.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}