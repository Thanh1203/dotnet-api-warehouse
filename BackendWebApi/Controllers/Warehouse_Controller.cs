using BackendWebApi.Helpers;
using BackendWebApi.Interfaces;
using BackendWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackendWebApi.Controllers
{
    [Route("api/Warehouse")]
    [ApiController]
    public class Warehouse_Controller(IWarehouse_Info IWarehouse_Info) : Controller
    {
        private readonly IWarehouse_Info _IWarehouse_Info = IWarehouse_Info;
        private readonly int companyId = GlobalConstant.CompanyId;

        [HttpGet]
        public async Task<IActionResult> FetchWarehousesInfo(string? name, string? nation, string? area)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(nation) && string.IsNullOrWhiteSpace(area))
                {
                    return Ok(await _IWarehouse_Info.GetWarehouseInfos(companyId));                    
                }
                else
                {
                   return Ok(await _IWarehouse_Info.SearchWarehouseInfo(name, nation, area, companyId));
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("nation")]
        public async Task<IActionResult> GetNationWarehouse()
        {
            try
            {
                var Nations = await _IWarehouse_Info.GetNationWarehouse(companyId);
                return Ok(Nations);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("area")]
        public async Task<IActionResult> GetAreaWarehouse()
        {
            try
            {
                var Areas = await _IWarehouse_Info.GetAreaWarehouse(companyId);
                return Ok(Areas);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateWarehouse([FromBody] Warehouse_Info warehouse)
        {
            try
            { 
                await _IWarehouse_Info.Create(warehouse, companyId);
                return Ok("Create successful!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateWarehouse([FromBody] Warehouse_Info warehouse)
        {
            try
            {
                await _IWarehouse_Info.Update(warehouse, companyId);
                return Ok("Update successful!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteWareHouse([FromBody] List<int> idsDelete)
        {
            try
            {
                await _IWarehouse_Info.Delete(idsDelete, companyId);
                return Ok("Delete successful!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
