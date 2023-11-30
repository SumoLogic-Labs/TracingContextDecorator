# TracingContextDecorator

TracingContextDecorator is a component that injects opentelemetry tracing context, e.g. traceid, spanid, parentspanid into log messsage.

### Supported log libraries

 - Log4net
 - Microsoft.Extensions.Logging
 - NLog
 - Serilog

### Steps needed to use TracingContextDecorator in the context of Log4net

1) Add Sumologic.TracingContextDecorator `dotnet add package Sumologic.TracingContextDecorator`
2) Update your app.config file with log4net section. Below is a complete example of app.config
   that defines log entry format and uses ConsoleAppender.

   More information about appenders is available at https://logging.apache.org/log4net/release/config-examples.html

```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <configSections>
        <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net" />
    </configSections>
    <log4net>
        <appender name="ConsoleAppender" type="log4net.Appender.ConsoleAppender" >
            <layout type="log4net.Layout.PatternLayout">
                <converter>
                    <name value="trace_id" />
                    <type value="SumoLogic.LoggingContext.TraceIdPatternConverter, TracingContextDecorator,Version=0.0.1, Culture=neutral, PublicKeyToken=null" />
                </converter>
                <converter>
                    <name value="span_id" />
                    <type value="SumoLogic.LoggingContext.SpanIdPatternConverter" />
                </converter>
                <converter>
                    <name value="parent_span_id" />
                    <type value="SumoLogic.LoggingContext.ParentSpanIdPatternConverter" />
                </converter>
                <conversionPattern value="%date %level %logger trace_id:%trace_id span_id:%span_id parent_span_id:%parent_span_id - %message%newline" />
            </layout>
        </appender>
        <root>
            <level value="INFO" />
            <appender-ref ref="ConsoleAppender" />
        </root>
    </log4net>
</configuration>
```

Below simple driver program that shows usage.

```
namespace UseLog4Net;
class Program
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Program));
    static void Main(string[] args)
    {

        log4net.Config.XmlConfigurator.Configure();
        log.Info("Main");
    }
}
```

###  Steps needed to use TracingContextDecorator in the context of Microsoft.Extensions.Logging

1) Add Sumologic.TracingContextDecorator `dotnet add package Sumologic.TracingContextDecorator`
2) Use TracingContextFormatter, below complete example

```
using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Logging.Console;
using SumoLogic.LoggingContext;

internal class Program
{
    static void Main(string[] args)
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole(options => options.FormatterName = "SumoLogic.LoggingContext.TracingContextFormatter")
                   .AddConsoleFormatter<TracingContextFormatter, ConsoleFormatterOptions>();
        });
        ILogger<Program> logger = loggerFactory.CreateLogger<Program>();
        logger.LogInformation("Hello World! Logging is {Description}.", "fun");
    }
}
```

### Steps needed to use NLog

1) Add Sumologic.TracingContextDecorator `dotnet add package Sumologic.TracingContextDecorator`
2) Define layout, below example one

```
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      xsi:schemaLocation="http://www.nlog-project.org/schemas/NLog.xsd NLog.xsd"
      autoReload="true"
      throwExceptions="false"
      internalLogLevel="Off" internalLogFile="C:\Windows\Temp\ConsoleApplication\nlog-internal.log" >
  <time type="FastUTC" />
  <targets>
    <target name="file" xsi:type="File"
              fileName="${basedir}/Logs/${shortdate}.log"
              layout="[${longdate}] [${uppercase:${level}}] [${logger}] ${message} ${exception:format=tostring}"
              concurrentWrites="false" keepFileOpen="false"/>
    <target name="console" xsi:type="ColoredConsole"
            layout="[${longdate}] [${uppercase:${level}}] [${logger:shortName=true}] [${nlog-tracing-context}]  ${message} ${exception:format=tostring}">
                 <highlight-row condition="level >= LogLevel.Info" foregroundColor="Blue" backgroundColor="NoChange"/>
    </target>
  </targets>
  <rules>
    <logger name="*" writeTo="console,file" />
  </rules>
</nlog>
```

Example NLogDriver

```
using System;
using NLog;
using NLog.LayoutRenderers;
namespace NLogDriver {
    public static class Program
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            LogManager.Setup().SetupExtensions(s => s.RegisterLayoutRenderer("nlog-tracing-context",typeof(NLogTracingContextLayoutRenderer)));
            Logger.Info("Application started...");
        }
    }
}
```

## Steps needed to use Serilog
1) Add Sumologic.TracingContextDecorator `dotnet add package Sumologic.TracingContextDecorator`
2) Implement client

```
using System;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using OpenTelemetry.Trace;
using SumoLogic.LoggingContext;

namespace SerilogDriver {

    public static class Program
    {
        public static void Main(string[] args)
        {
            var loggerConfig = new LoggerConfiguration().MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .Enrich.With(new SerilogTracingContextEnricher())
                .WriteTo.Console(
                         outputTemplate: "[trace_id: {trace_id} span_id: {span_id} parent_span_id: {parent_span_id} {Level:u3} {Subsystem}] {Message:lj}{NewLine}{Exception}",
                         restrictedToMinimumLevel: LogEventLevel.Information,
                         theme: AnsiConsoleTheme.Sixteen);
            var log  = loggerConfig.CreateLogger();
            log.Information("Hello, Serilog!");
        }
    }
}
```

3) Build Project
