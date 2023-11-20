# TracingContextDecorator

TracingContextDecorator is a component that injects opentelemetry tracing context, e.g. traceid, spanid, parentspanid into log messsage.

### Supported log libraries

 - Log4net

### Steps needed to use TracingContextDecorator

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

3) Build Project
