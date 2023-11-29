using System.Text;
using NLog;
using NLog.LayoutRenderers;
using OpenTelemetry.Trace;

[LayoutRenderer("nlog-tracing-context")]
public class NLogTracingContextLayoutRenderer : LayoutRenderer
{
    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
        builder.Append("trace_id: " + Tracer.CurrentSpan.Context.TraceId + " ");
        builder.Append("span_id: " + Tracer.CurrentSpan.Context.SpanId + " ");
        builder.Append("parent_span_id: " + Tracer.CurrentSpan.ParentSpanId);
    }
}
