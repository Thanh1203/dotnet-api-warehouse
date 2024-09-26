using BackendWebApi.DTOS;
using BackendWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackendWebApi.Interfaces
{
    public interface IWarehouse_Data
    {
        Task UpdateUnitPrice(int idWarehouse, int productId, double unitPrice, int companyId);
        Task InsertProduct(DTOWarehouseData_Create data_Create, int companyId);
        Task UpdateQuantityProduct(DTOWarehouseData_Update data_Update, int companyId);
        Task DecreaseQuantityProduct(DTOWarehouseData_Update data_Update, DTOCustomer customer, int companyId);
    }
}
