using System;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using SumoLogic.LoggingContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SerilogTracingContext.Pages;

public class IndexModel : PageModel
{
    private readonly Serilog.Core.Logger log;

    public IndexModel(ILogger<IndexModel> logger)
    {
            var loggerConfig = new LoggerConfiguration().MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .Enrich.With(new SerilogTracingContextEnricher())
                .WriteTo.Console(
                         outputTemplate: "[trace_id: {trace_id} span_id: {span_id} parent_span_id: {parent_span_id} {Level:u3} {Subsystem}] {Message:lj}{NewLine}{Exception}",
                         restrictedToMinimumLevel: LogEventLevel.Information,
                         theme: AnsiConsoleTheme.Sixteen);
            log = loggerConfig.CreateLogger();
            log.Information("Hello, Serilog!");
    }

    public void OnGet()
    {

    }
}
