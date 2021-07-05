# VSLauncher 
Simple Visual Studio launcher designed to work with [RepoZ](https://github.com/awaescher/RepoZ)

[![Build Status](https://dev.azure.com/sboulema/VSLauncher/_apis/build/status/VSLauncher?branchName=main)](https://dev.azure.com/sboulema/VSLauncher/_build/latest?definitionId=28&branchName=main)
[![Sponsor](https://img.shields.io/badge/-Sponsor-fafbfc?logo=GitHub%20Sponsors)](https://github.com/sponsors/sboulema)

## Usage
To use VSLauncher to quickly open a Visual Studio solution using Repoz edit the RepositoryActions.json file, by adding the follow json block to the `repository-actions` array

```
{
    "name": "{OpenIn} Visual Studio",
    "executables": [ '<path to VSLauncher>/VSLauncher.exe' ],
    "arguments": '"{Repository.SafePath}"',  
    "active": "true"
},
```

Read more about custom repository actions at the [RepoZ Community Repository Actions GitHub](https://github.com/awaescher/RepoZ-RepositoryActions).