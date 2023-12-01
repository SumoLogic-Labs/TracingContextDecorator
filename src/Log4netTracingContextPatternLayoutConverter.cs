using System;
using System.IO;
using log4net.Layout.Pattern;
using OpenTelemetry.Trace;

namespace SumoLogic.LoggingContext;

public class Log4netTraceIdPatternLayoutConverter : PatternLayoutConverter
{
    protected override void Convert(TextWriter writer, log4net.Core.LoggingEvent loggingEvent)
    {
        writer.Write(Tracer.CurrentSpan.Context.TraceId);
    }
}
public class Log4netSpanIdPatternLayoutConverter : PatternLayoutConverter
{
    protected override void Convert(TextWriter writer, log4net.Core.LoggingEvent loggingEvent)
    {
        writer.Write(Tracer.CurrentSpan.Context.SpanId);
    }
}
public class Log4netParentSpanIdPatternLayoutConverter : PatternLayoutConverter
{
    protected override void Convert(TextWriter writer, log4net.Core.LoggingEvent loggingEvent)
    {
        writer.Write(Tracer.CurrentSpan.ParentSpanId);
    }
}
