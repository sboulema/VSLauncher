using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

var folder = args.FirstOrDefault();

if (string.IsNullOrEmpty(folder))
{
    Console.WriteLine("Folder path must be specified!");
    return;
}

if (!Directory.Exists(folder))
{
    Console.WriteLine("Unable to find specified directory!");
    return;
}

var solutionFilePath = Directory
    .GetFiles(folder, "*.sln")
    .FirstOrDefault();

if (string.IsNullOrEmpty(solutionFilePath))
{
    Console.WriteLine("Unable to find a VS solution!");
    return;
}

// Fix any mixed path seperators
solutionFilePath = Path.GetFullPath(solutionFilePath);

Console.WriteLine($"Opening '{solutionFilePath}'");

new Process
{
    StartInfo = new()
    {
        FileName = solutionFilePath,
        UseShellExecute = true,
        CreateNoWindow = true
    }
}.Start();