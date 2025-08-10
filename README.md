# <img src="https://github.com/CodeShayk/ApiAggregator/blob/master/Images/ninja-icon-16.png" alt="ninja" style="width:30px;"/> ApiAggregator v2.0.1
[![NuGet version](https://badge.fury.io/nu/ApiAggregator.svg)](https://badge.fury.io/nu/ApiAggregator) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/CodeShayk/ApiAggregator/blob/master/LICENSE.md) 
[![Master-Build](https://github.com/CodeShayk/ApiAggregator/actions/workflows/Master-Build.yml/badge.svg)](https://github.com/CodeShayk/ApiAggregator/actions/workflows/Master-Build.yml) 
[![GitHub Release](https://img.shields.io/github/v/release/CodeShayk/ApiAggregator?logo=github&sort=semver)](https://github.com/CodeShayk/ApiAggregator/releases/latest)
[![Master-CodeQL](https://github.com/CodeShayk/ApiAggregator/actions/workflows/Master-CodeQL.yml/badge.svg)](https://github.com/CodeShayk/ApiAggregator/actions/workflows/Master-CodeQL.yml) 
[![.Net 9.0](https://img.shields.io/badge/.Net-9.0-blue)](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
--
## Introduction
### What is ApiAggregator?
`ApiAggregator` is a .net utility to help combine multiple api requests to return a single aggregated response. 
The framework allows fetching the whole of aggregated response or a partial response based on the list of configured apis included in the aggregator request.

### When is ApiAggregator useful?
ApiAggregator is useful in many use cases. Few to list are:
- For creating Level 2 (functional or BFF) apis using Level 1 (core resource) apis.
- For easily extending an api without having to break existing consumers.
- For on demand retrieval of data using different subsets of configured apis to fetch varied datasets per request.
- and Many more.

## Getting Started?
### i. Installation
Install the latest version of ApiAggregator nuget package with command below. 

```
NuGet\Install-Package ApiAggregator 
```

### ii. Developer Guide
This comprehensive guide provides detailed information about the ApiAggregator framework, covering everything from basic concepts to advanced implementation patterns and troubleshooting guidelines.

Please click on [Developer Guide](https://github.com/CodeShayk/ApiAggregator/wiki/Developer-Guide) for complete details.


## Support

If you are having problems, please let me know by [raising a new issue](https://github.com/CodeShayk/ApiAggregator/issues/new/choose).

## License

This project is licensed with the [MIT license](LICENSE).

## Version History
The main branch is now on .NET 9.0. The following previous versions are available:
| Version  | Release Notes |
| -------- | --------|
| [`v2.0.0`](https://github.com/CodeShayk/ApiAggregator/tree/v2.0.0) |  [Notes](https://github.com/CodeShayk/ApiAggregator/releases/tag/v2.0.0) |
| [`v1.0.0`](https://github.com/CodeShayk/ApiAggregator/tree/v1.0.0) |  [Notes](https://github.com/CodeShayk/ApiAggregator/releases/tag/v1.0.0) |

## Credits
Thank you for reading. Please fork, explore, contribute and report. Happy Coding !! :)
