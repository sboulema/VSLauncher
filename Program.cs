using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace VSLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!args.Any())
            {
                Console.WriteLine("Folder path must be specified!");
                return;
            }

            var folder = args.FirstOrDefault();

            if (!Directory.Exists(folder))
            {
                Console.WriteLine("Unable to find specified directory!");
                return;
            }

            var files = Directory.GetFiles(folder, "*.sln");
            var sln = files.FirstOrDefault();

            if (string.IsNullOrEmpty(sln))
            {
                Console.WriteLine("Unable to find a VS solution!");
                return;
            }

            Console.WriteLine($"Opening {sln}");

            OpenSln(sln);
        }

        private static bool OpenSln(string path)
            => new Process
            {
                StartInfo = new()
                {
                    FileName = path,
                    UseShellExecute = true,
                    CreateNoWindow = true
                }
            }.Start();
    }
}
