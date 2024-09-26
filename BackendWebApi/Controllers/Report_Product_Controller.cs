using BackendWebApi.Helpers;
using BackendWebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackendWebApi.Controllers
{
    [Route("api/report_product")]
    [ApiController]
    public class Report_Product_Controller(IReportProduct reportProduct) : Controller
    {
        private readonly IReportProduct _IReportProduct = reportProduct;
        private readonly int companyId = GlobalConstant.CompanyId;

        [HttpGet]
        public async Task<IActionResult> FetchDataReportProduct()
        {
            try
            {
                var data = await _IReportProduct.GetReportProduct(companyId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("excess_inventory")]
        public async Task<IActionResult> FetchDataReportSaleProduct(int year, int? month)
        {
            try
            {
                var data = await _IReportProduct.GetReportSaleProduct(companyId, year, month);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
