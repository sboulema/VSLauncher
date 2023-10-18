using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

const string _vswhereFilePath = "C:\\Program Files (x86)\\Microsoft Visual Studio\\Installer\\vswhere.exe";

var directory = args.FirstOrDefault();

if (string.IsNullOrEmpty(directory))
{
    Console.WriteLine("Folder path must be specified!");
    return;
}

if (!Directory.Exists(directory))
{
    Console.WriteLine("Unable to find specified directory!");
    return;
}

var filePath = GetFilePath(directory, "*.sln");

if (!string.IsNullOrEmpty(filePath))
{
    Console.WriteLine($"Opening solution '{filePath}'");
    Run(filePath, true);
    return;
}

var vsPath = RunWithResults(_vswhereFilePath, "-prerelease -latest -property productPath");

filePath = GetFilePath(directory, "*.*proj");

if (!string.IsNullOrEmpty(filePath))
{
    Console.WriteLine($"Opening project '{filePath}'");
    Run(vsPath, arguments: filePath);
    return;
}

Console.WriteLine($"Opening directory '{directory}'");
Run(vsPath, arguments: directory);

static bool Run(string filePath, bool useShellExecute = false, string arguments = "")
    => new Process
    {
        StartInfo = new()
        {
            FileName = filePath,
            Arguments = arguments,
            UseShellExecute = useShellExecute,
            CreateNoWindow = true
        }
    }.Start();

static string RunWithResults(string filePath, string arguments)
{
    var process = new Process
    {
        StartInfo = new()
        {
            FileName = filePath,
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        }
    };

    process.Start();

    string output = string.Empty;

    while (!process.StandardOutput.EndOfStream)
    {
        output += process.StandardOutput.ReadLine();
    }

    return output;
}

static string GetFilePath(string directory, string searchPattern)
{
    var filePath = Directory
        .EnumerateFiles(directory, searchPattern)
        .FirstOrDefault();

    if (string.IsNullOrEmpty(filePath))
    {
        return string.Empty;
    }

    return Path.GetFullPath(filePath);
}