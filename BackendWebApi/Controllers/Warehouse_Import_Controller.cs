using BackendWebApi.Helpers;
using BackendWebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackendWebApi.Controllers
{
    [Route("api/WarehouseImport")]
    [ApiController]
    public class Warehouse_Import_Controller(IWarehouse_Import warehouse_Import) : Controller
    {
        private readonly IWarehouse_Import _warehouse_Import = warehouse_Import;
        private readonly int companyId = GlobalConstant.CompanyId;

        [HttpGet("{warehouseId}")]
        public async Task<IActionResult> FetchHistoryImpor(int warehouseId, string? day, string? month, string? year)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(day) && string.IsNullOrWhiteSpace(month) && string.IsNullOrWhiteSpace(year))
                {
                    return Ok(await _warehouse_Import.GetHistoryImport(warehouseId, companyId));
                }
                else
                {
                    return Ok(await _warehouse_Import.SearHistoryImport(day, month, year, warehouseId, companyId));
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
