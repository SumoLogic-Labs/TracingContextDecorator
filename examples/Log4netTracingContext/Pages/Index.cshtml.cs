using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Log4netTracingContext.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(IndexModel));
    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
	log.Info("OnGet");
    }
}
