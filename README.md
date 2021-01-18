# RemoteBrowserMobProxy

This is a netstandard2.0 client library to communiate with BrowserMobProxy server API described at https://github.com/lightbody/browsermob-proxy

Example of usage (Docker needs to be installed):
- Run the [compose.ps1](./compose.ps1) powerhell script to prepare selenium hub, node and the proxy
- See example of communication with browsermobproxy docker container [RemoteBrowserMobProxyFunctionalTests.cs](./RemoteBrowserMobProxy.Tests/RemoteBrowserMobProxyFunctionalTests.cs)

[![NuGet Status](http://nugetstatus.com/RemoteBrowserMobProxy.png)](http://nugetstatus.com/packages/RemoteBrowserMobProxy)
[![Build Status](https://ci.appveyor.com/api/projects/status/ur3s4k4d80i0nphb/branch/master?svg=true)](https://ci.appveyor.com/project/tmulkern/remotebrowsermobproxy/branch/master)
[![codecov](https://codecov.io/gh/tmulkern/RemoteBrowserMobProxy/branch/master/graph/badge.svg)](https://codecov.io/gh/tmulkern/RemoteBrowserMobProxy)
