using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Log4netTracingContext.Pages;

public class IndexModel : PageModel
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(IndexModel));
    public IndexModel(ILogger<IndexModel> logger)
    {
        log4net.Config.XmlConfigurator.Configure();
    }

    public void OnGet()
    {
	log.Info("OnGet");
    }
}
