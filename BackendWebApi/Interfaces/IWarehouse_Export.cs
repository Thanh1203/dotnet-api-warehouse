namespace BackendWebApi.Interfaces
{
    public interface IWarehouse_Export
    {
        Task<object> GetWeareHouseExport(int warehouseId, int companyId);
        Task<object> SearchWarehouseExport(string day, string month, string year, int warehouseId, int companyId);
    }
}
