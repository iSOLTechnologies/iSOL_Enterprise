using Microsoft.AspNetCore.Mvc;

namespace iSOL_Enterprise.Controllers.Reports
{
    public class SaleOrderWiseYarnReportController : Controller
    {
        IConfiguration _configuration;
        public SaleOrderWiseYarnReportController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            ViewBag.Url = _configuration["ReportUrl"].ToString();
            return View();
        }
    }
}
