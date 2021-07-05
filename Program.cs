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

            var filePaths = Directory.GetFiles(folder, "*.sln");
            var solutionFilePath = filePaths.FirstOrDefault();

            if (string.IsNullOrEmpty(solutionFilePath))
            {
                Console.WriteLine("Unable to find a VS solution!");
                return;
            }

            // Fix any mixed path seperators
            solutionFilePath = Path.GetFullPath(solutionFilePath);

            Console.WriteLine($"Opening '{solutionFilePath}'");

            OpenVisualStudioSolution(solutionFilePath);
        }

        private static bool OpenVisualStudioSolution(string path)
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
