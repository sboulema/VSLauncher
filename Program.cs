using System;
using System.CommandLine;
using System.Diagnostics;
using System.IO;
using System.Linq;

var directoryArgument = new Argument<string>("directory", "Directory in which to open solution/project/directory.");
var versionOption = new Option<string?>(new[] { "--vs-version", "-v" }, "The version of Visual Studio to launch.");
var prereleaseOption = new Option<bool>(new[] { "--prerelease", "-p" }, "The release channel of Visual Studio to launch.");

var rootCommand = new RootCommand("Visual Studio Launcher")
{ 
    directoryArgument,
    versionOption,
    prereleaseOption
};

rootCommand.SetHandler(LaunchVisualStudio,
    directoryArgument,
    versionOption,
    prereleaseOption);

return await rootCommand.InvokeAsync(args);

static void LaunchVisualStudio(string directory, string? vsVersion, bool prerelease)
{
    if (!Directory.Exists(directory))
    {
        Console.WriteLine("Unable to find specified directory!");
        return;
    }

    var filePath = GetFilePath(directory, "*.sln");

    var vsPath = GetVisualStudioPath(vsVersion, prerelease);

    if (string.IsNullOrEmpty(vsPath))
    {
        Console.WriteLine("Unable to find matching Visual Studio installation!");
        return;
    }

    if (!string.IsNullOrEmpty(filePath))
    {
        Console.WriteLine($"Opening solution '{filePath}'");
        Run(vsPath, filePath);
        return;
    }

    filePath = GetFilePath(directory, "*.*proj");

    if (!string.IsNullOrEmpty(filePath))
    {
        Console.WriteLine($"Opening project '{filePath}'");
        Run(vsPath, filePath);
        return;
    }

    Console.WriteLine($"Opening directory '{directory}'");
    Run(vsPath, directory);
}

static bool Run(string vsPath, string arguments)
    => new Process
    {
        StartInfo = new()
        {
            FileName = vsPath,
            Arguments = arguments,
            UseShellExecute = false,
            CreateNoWindow = true
        }
    }.Start();

static string GetVisualStudioPath(string? vsVersion, bool prerelease)
{
    var versionArgument = vsVersion switch
    {
        "2017" => "-version [15,16)",
        "2019" => "-version [16,17)",
        "2022" => "-version [17,18)",
        _ => "-latest"
    };

    var arguments = $"{(prerelease ? "-prerelease": string.Empty)} {versionArgument} -property productPath";

    var process = new Process
    {
        StartInfo = new()
        {
            FileName = "C:\\Program Files (x86)\\Microsoft Visual Studio\\Installer\\vswhere.exe",
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