using BackendWebApi.Models;

namespace BackendWebApi.Interfaces
{
    public interface IWarehouse_Info
    {
        Task<object> GetWarehouseInfos(int companyId);
        Task<object> SearchWarehouseInfo(string name, string nation, string area, int companyId);
        Task<List<string>> GetNationWarehouse(int companyId);
        Task<List<string>> GetAreaWarehouse(int companyId);
        Task Create (Warehouse_Info warehouse, int companyId);
        Task Update (Warehouse_Info warehouse, int companyId);
        Task Delete (List<int> ids, int companyId);
    }
}
