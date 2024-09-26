using BackendWebApi.Helpers;
using BackendWebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackendWebApi.Controllers
{
    [Route("api/WarehouseExport")]
    [ApiController]
    public class Warehouse_Export_Controller(IWarehouse_Export warehouse_Export) : Controller
    {
        private readonly IWarehouse_Export _warehouse_Export = warehouse_Export;
        private readonly int companyId = GlobalConstant.CompanyId;

        [HttpGet("{warehouseId}")]
        public async Task<IActionResult> FetchWarehouseExport(int warehouseId, string? day, string? month, string? year)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(day) && string.IsNullOrWhiteSpace(month) && string.IsNullOrWhiteSpace(year))
                {
                    return Ok(await _warehouse_Export.GetWeareHouseExport(warehouseId, companyId));
                }
                else
                {
                    return Ok(await _warehouse_Export.SearchWarehouseExport(day, month, year, warehouseId, companyId));
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
