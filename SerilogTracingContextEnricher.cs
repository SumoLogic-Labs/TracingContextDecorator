using Serilog;
using Serilog.Core;
using Serilog.Events;
using OpenTelemetry.Trace;

namespace SerilogDriver {
    public class SerilogTracingContextEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory pf)
        {
            logEvent.AddPropertyIfAbsent(pf.CreateProperty("trace_id", Tracer.CurrentSpan.Context.TraceId));
            logEvent.AddPropertyIfAbsent(pf.CreateProperty("span_id", Tracer.CurrentSpan.Context.SpanId));
            logEvent.AddPropertyIfAbsent(pf.CreateProperty("parent_span_id", Tracer.CurrentSpan.ParentSpanId));
        }
    }
}
