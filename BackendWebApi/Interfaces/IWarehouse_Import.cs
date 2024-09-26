namespace BackendWebApi.Interfaces
{
    public interface IWarehouse_Import
    {
        Task<object> GetHistoryImport(int warehouseId, int companyId);
        Task<object> SearHistoryImport(string day, string month, string year, int warehouseId, int companyId);
    }
}
