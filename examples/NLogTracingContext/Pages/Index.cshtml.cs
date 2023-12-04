using NLog;
using NLog.LayoutRenderers;
using SumoLogic.LoggingContext;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NLogTracingContext.Pages;

public class IndexModel : PageModel
{
    private static readonly NLog.ILogger Log = LogManager.GetCurrentClassLogger();
    public IndexModel(ILogger<IndexModel> logger)
    {
        LogManager.Setup().SetupExtensions(s => s.RegisterLayoutRenderer("nlog-tracing-context",typeof(NLogTracingContextLayoutRenderer)));
        Log.Info("Application started...");
    }

    public void OnGet()
    {
	Log.Info("OnGet...");
    }
}
