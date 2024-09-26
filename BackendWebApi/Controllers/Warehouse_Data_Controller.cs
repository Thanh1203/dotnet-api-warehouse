using BackendWebApi.DTOS;
using BackendWebApi.Helpers;
using BackendWebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace BackendWebApi.Controllers
{
    [Route("api/warehouseData")]
    [ApiController]
    public class Warehouse_Data_Controller(IWarehouse_Data IWarehouse_Data) : Controller
    {
        private readonly IWarehouse_Data _IWarehouse_Data  = IWarehouse_Data;
        private readonly int companyId = GlobalConstant.CompanyId;


        [HttpPut("{warehouseId}")]
        public async Task<IActionResult> UpdateWarehouseData(int warehouseId, [FromBody] DTOWarehouse_Update_UnitPrice value)
        {
            try
            {
                await _IWarehouse_Data.UpdateUnitPrice(warehouseId, value.ProductId, value.UntiPirce, companyId);
                return Ok("Update successful!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertData([FromBody] DTOWarehouseData_Create? data_Create)
        {
            try
            {
                await _IWarehouse_Data.InsertProduct(data_Create, companyId);
                return Ok("Create successful!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut("GoodsImport")]
        public async Task<IActionResult> UpdateData([FromBody] DTOWarehouseData_Update? data_Update)
        {
            try
            {
                await _IWarehouse_Data.UpdateQuantityProduct(data_Update, companyId);
                return Ok("Update successful!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut("GoodsExport")]
        public async Task<IActionResult> DecreaseData([FromBody] DTOWh_Cus dtoData)
        {
            try
            {
                await _IWarehouse_Data.DecreaseQuantityProduct(dtoData.DataUpdate, dtoData?.Customer, companyId);
                return Ok("Update successful!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }

}
