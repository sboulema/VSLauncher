# VSLauncher 
Simple Visual Studio launcher designed to work with [RepoZ](https://github.com/awaescher/RepoZ)

[![VSLauncher](https://github.com/sboulema/VSLauncher/actions/workflows/workflow.yml/badge.svg)](https://github.com/sboulema/VSLauncher/actions/workflows/workflow.yml)
[![Sponsor](https://img.shields.io/badge/-Sponsor-fafbfc?logo=GitHub%20Sponsors)](https://github.com/sponsors/sboulema)

## Features
- Open a solution file from the specified directory in Visual Studio 
- Open a project file from the specified directory in Visual Studio
- Open the specified directory in Visual Studio

## Usage

### CLI
`VSLauncher.exe <directory> [options]`

| Name        | Parameter         | Required | Example             | Description |
| ----------- | ----------------- | -------- | ------------------- | ----------- |
| Directory   |                   | yes      | D:\Repos\VSLauncher | Directory in which to open solution/project/directory |
| Version     | --vs-version, -v  | no       | 2017, 2019, 2022    | The version of Visual Studio to launch |
| Prerelease  | --prerelease, -p  | no       |                     | The release channel of Visual Studio to launch |
| Interactive | --interactive, -i | no       |                     | Interactively choose which version of Visual Studio to launch |
| Help        | --help, -h, -?    | no       |                     | Show help |

### RepoZ
To use VSLauncher to quickly open a Visual Studio solution using RepoZ edit the RepositoryActions.json file, by adding the following json block to the `repository-actions` array

```
{
    "name": "{OpenIn} Visual Studio",
    "executables": [ '<path to VSLauncher>/VSLauncher.exe' ],
    "arguments": '"{Repository.SafePath}"',  
    "active": "true"
},
```

Read more about custom repository actions at the [RepoZ Community Repository Actions GitHub](https://github.com/awaescher/RepoZ-RepositoryActions).

## Links
- [What is the difference between Directory.EnumerateFiles vs Directory.GetFiles?](https://stackoverflow.com/questions/5669617/what-is-the-difference-between-directory-enumeratefiles-vs-directory-getfiles)
- [Visual Studio Locator](https://github.com/Microsoft/vswhere)
- [vswhere now searches older versions of Visual Studio](https://devblogs.microsoft.com/setup/vswhere-now-searches-older-versions-of-visual-studio/)