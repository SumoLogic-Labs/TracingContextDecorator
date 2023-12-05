# TracingContextDecorator

TracingContextDecorator is a component that enriches logs with opentelemetry tracing context e.g. traceid, spanid, parentspanid into log messsage.
Implementation of required abstractions for specific libraries lives inside `src` subdirectory. All examples are inside `examples` subdirectory.

### Supported log libraries

There are four supported libraries. Each of them uses different abstractions to achieve the goal.

 - Log4net
 - Microsoft.Extensions.Logging
 - NLog
 - Serilog

Log4net requires `PatternLayoutConverter` implementation and its configuration in the context of specific appender.

Microsoft.Extensions.Logging requires specific implementation of formatter. This package contains implementation of console formatter.

NLog requires implementation of `LayoutRenderer`.

Serilog requires implementation of `ILogEventEnricher` interface.

Above components can be configured via code or config file. Examples for Log4net and NLog show how to do that via configuration.

### Usage

Using `TracingContextDecorator` requires addition of dependencies. Dependendencies can be added via

`dotnet add package` or `dotnet add reference`. You can learn what's needed looking into examples or

reading documentation for specific log library. Below dependencies excerpt from serilog example:

```
  <ItemGroup>
    <PackageReference Include="Serilog.Sinks.Console" Version="5.0.1" />
  </ItemGroup>^M
  <ItemGroup>
    <ProjectReference Include="..\..\src\TracingContextDecorator.csproj" />
  </ItemGroup>^M
```
