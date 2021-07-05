# VSLauncher 
Simple Visual Studio launcher designed to worj with RepoZ

[![Build Status](https://dev.azure.com/sboulema/CodeNav/_apis/build/status/CodeNav?branchName=main)](https://dev.azure.com/sboulema/CodeNav/_build/latest?definitionId=26&branchName=main)
[![Sponsor](https://img.shields.io/badge/-Sponsor-fafbfc?logo=GitHub%20Sponsors)](https://github.com/sponsors/sboulema)

## Usage
To use VSLauncher to quickly open a Visual Studio solution using Repoz edit the RepositoryActions.json file, by adding the follow json block to the `repository-actions` array

```
{
    "name": "{OpenIn} Visual Studio",
    "executables": [ 'C:/Repos/VSLauncher/bin/Release/net5.0/publish/VSLauncher.exe' ],
    "arguments": '"{Repository.SafePath}"',  
    "active": "true"
},
```