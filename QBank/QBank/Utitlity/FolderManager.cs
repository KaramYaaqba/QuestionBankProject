using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qrator.Utility
{
    public static class FolderManager
    {
        public static string DefineFolderName()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
        }
    }
}
