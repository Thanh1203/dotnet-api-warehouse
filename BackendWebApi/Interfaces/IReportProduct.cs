namespace BackendWebApi.Interfaces
{
    public interface IReportProduct
    {
        public Task<object> GetReportProduct(int companyId);
        public Task<object> GetReportSaleProduct(int companyId, int year, int? month);
    }
}
