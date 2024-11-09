# <img src="https://github.com/CodeShayk/ApiAggregator/blob/master/Images/ninja-icon-16.png" alt="ninja" style="width:30px;"/> ApiAggregator v1.0 
[![NuGet version](https://badge.fury.io/nu/ApiAggregator.svg)](https://badge.fury.io/nu/ApiAggregator) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/CodeShayk/ApiAggregator/blob/master/LICENSE.md) 
[![Master-Build](https://github.com/CodeShayk/ApiAggregator/actions/workflows/Master-Build.yml/badge.svg)](https://github.com/CodeShayk/ApiAggregator/actions/workflows/Master-Build.yml) 
[![GitHub Release](https://img.shields.io/github/v/release/CodeShayk/ApiAggregator?logo=github&sort=semver)](https://github.com/CodeShayk/ApiAggregator/releases/latest)
[![Master-CodeQL](https://github.com/CodeShayk/ApiAggregator/actions/workflows/Master-CodeQL.yml/badge.svg)](https://github.com/CodeShayk/ApiAggregator/actions/workflows/Master-CodeQL.yml) 
[![.Net 8.0](https://img.shields.io/badge/.Net-8.0-blue)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
--
## Introduction
### What is ApiAggregator?
`ApiAggregator` is a .net utility to help combine multiple api requests to return a single aggregated response. 
The framework allows fetching the whole of aggregated response or a partial response based on the list of configured apis included in the aggregator request.

### When is ApiAggregator useful?
ApiAggregator is useful in various use cases. 
- For creating Level 2 (functional or BFF) apis using Level 1 (core resource) apis.
- For easily extending an api without having to break existing consumers.
- For on demand data retrieval using list of apis
- and Many more.

## Getting Started?
### i. Installation
Install the nuget package as below. 

```
NuGet\Install-Package ApiAggregator 
```

### ii. Implementation Guide

Please read [Implementation Guide](https://github.com/CodeShayk/ApiAggregator/blob/master/ApiAggregator.md) for details on how to implement ApiAggregator in your project.

## Support

If you are having problems, please let me know by [raising a new issue](https://github.com/CodeShayk/ApiAggregator/issues/new/choose).

## License

This project is licensed with the [MIT license](LICENSE).

## Version History
The main branch is now on .NET 8.0. The following previous versions are available:
| Version  | Release Notes |
| -------- | --------|
| [`v1.0.0`](https://github.com/CodeShayk/ApiAggregator/tree/v1.0.0) |  [Notes](https://github.com/CodeShayk/ApiAggregator/releases/tag/v1.0.0) |

## Credits
Thank you for reading. Please fork, explore, contribute and report. Happy Coding !! :)
