using BackendWebApi.Models;

namespace BackendWebApi.Interfaces
{
    public interface IProduct_Info
    {
        Task<object> GetProduct_Infos(int companyId);
        Task<object> SearchProductInfo(string name, string categoryId, string classifyId, string producerId, int companyId);
        Task Create(Product_Info product_info, int companyId);
        Task Update(Product_Info product_info, int companyId);
        Task Delete(List<int> ids, int companyId);
        Task<object> GetProductOutsideWH(int warehouseId, int companyId);
        Task<object> GetProductInsideWH(int warehouseId, int companyId);
        Task<object> SearchProductInsideWH(int warehouseId, string code, int companyId);
        Task<object> GetProductConfigUnitPrice(int warehouseId, int companyId);
        Task<object> SearchProductConfigUnitPrice(int warehouseId, string code, string name, int companyId);
    }
}
