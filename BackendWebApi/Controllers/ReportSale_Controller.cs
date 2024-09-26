using BackendWebApi.Helpers;
using BackendWebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackendWebApi.Controllers
{
    [Route("api/report_product")]
    [ApiController]
    public class ReportSale_Controller(IReportSale reportSale) : Controller
    {
        private readonly IReportSale _reportSale = reportSale;
        private readonly int companyId = GlobalConstant.CompanyId;

        [HttpGet("{curreantYear}")]
        public async Task<IActionResult> FetchData(int curreantYear)
        {
            try
            {
                var data = await _reportSale.GetDataReportSale(companyId, curreantYear);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
