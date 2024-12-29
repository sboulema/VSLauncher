using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using VSLauncher.Models;

var directoryArgument = new Argument<string>("directory", "Directory in which to open solution/project/directory.");
var versionOption = new Option<string?>(["--vs-version", "-v"], "The version of Visual Studio to launch.");
var prereleaseOption = new Option<bool>(["--prerelease", "-p"], "The release channel of Visual Studio to launch.");
var interactiveOption = new Option<bool>(["--interactive", "-i"], "Interactively choose which version of Visual Studio to launch.");
var recursiveOption = new Option<bool>(["--recursive", "-r"], "Recursively search for a solution or project in the given directory and its children.");

var rootCommand = new RootCommand("Visual Studio Launcher")
{
	directoryArgument,
	versionOption,
	prereleaseOption,
	interactiveOption,
	recursiveOption
};

rootCommand.SetHandler(LaunchVisualStudio,
	directoryArgument,
	versionOption,
	prereleaseOption,
	interactiveOption,
	recursiveOption);

return await rootCommand.InvokeAsync(args);

static void LaunchVisualStudio(string directory, string? vsVersion,
	bool prerelease, bool interactive, bool recursive)
{
	if (!Directory.Exists(directory))
	{
		Console.WriteLine("Unable to find specified directory!");
		return;
	}

	var vsPath = GetVisualStudioPath(vsVersion, prerelease, interactive);

	if (string.IsNullOrEmpty(vsPath))
	{
		Console.WriteLine("Unable to find matching Visual Studio installation!");
		return;
	}

	var filePath = GetFilePath(directory, "*.sln", recursive);

	if (!string.IsNullOrEmpty(filePath))
	{
		Console.WriteLine($"Opening solution '{filePath}'");
		Run(vsPath, filePath);
		return;
	}

	filePath = GetFilePath(directory, "*.*proj", recursive);

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

static string GetVisualStudioPath(string? vsVersion, bool prerelease, bool interactive)
{
	if (interactive)
	{
		var vsInstallations = RunVisualStudioLocator("-prerelease");

		Console.WriteLine("Visual Studio Launcher");
		Console.WriteLine("Please select which installation of Visual Studio to launch");

		for (int i = 0; i < vsInstallations.Count; i++)
		{
			Console.WriteLine($"{i + 1}: {vsInstallations[i].DisplayName} ({vsInstallations[i].Catalog.ProductDisplayVersion})");
		}

		var indexString = Console.ReadLine();

		if (int.TryParse(indexString, out var index) == false)
		{
			return string.Empty;
		}

		return vsInstallations[index - 1].ProductPath;
	}

	var versionArgument = vsVersion switch
	{
		"2017" => "-version [15,16)",
		"2019" => "-version [16,17)",
		"2022" => "-version [17,18)",
		_ => "-latest"
	};

	var arguments = $"{(prerelease ? "-prerelease": string.Empty)} {versionArgument} -property productPath";

	return RunVisualStudioLocator(arguments)
		.FirstOrDefault()?.ProductPath ?? string.Empty;
}

static List<VisualStudio> RunVisualStudioLocator(string arguments)
{
	var process = new Process
	{
		StartInfo = new()
		{
			FileName = "C:\\Program Files (x86)\\Microsoft Visual Studio\\Installer\\vswhere.exe",
			Arguments = arguments += " -format json",
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

	return JsonSerializer.Deserialize(output, SourceGenerationContext.Default.ListVisualStudio) ?? [];
}

static string GetFilePath(string directory, string searchPattern, bool recursive)
{
	var filePath = Directory
		.EnumerateFiles(directory, searchPattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
		.FirstOrDefault();

	if (string.IsNullOrEmpty(filePath))
	{
		return string.Empty;
	}

	return Path.GetFullPath(filePath);
}

[JsonSerializable(typeof(List<VisualStudio>))]
internal partial class SourceGenerationContext : JsonSerializerContext { }