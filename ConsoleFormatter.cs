using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using OpenTelemetry.Trace;

namespace SumoLogic.LoggingContext;

public sealed class TracingContextFormatter : ConsoleFormatter
{

    public TracingContextFormatter()
        // Case insensitive
        : base("SumoLogic.LoggingContext." + nameof(TracingContextFormatter))
    {
    }

    public override void Write<TState>(
        in LogEntry<TState> logEntry,
        IExternalScopeProvider? scopeProvider,
        TextWriter textWriter)
    {
        string message =
            logEntry.Formatter(
                logEntry.State, logEntry.Exception);

        if (message == null)
        {
            return;
        }
        WritePrefix(textWriter);
        textWriter.WriteLine(message);
    }

    private void WritePrefix(TextWriter textWriter)
    {
        textWriter.Write($"trace_id:{Tracer.CurrentSpan.Context.TraceId} ");
        textWriter.Write($"span_id:{Tracer.CurrentSpan.Context.SpanId} ");
        textWriter.Write($"parent_span_id:{Tracer.CurrentSpan.ParentSpanId} ");
    }
}
