namespace BackendWebApi.Interfaces
{
    public interface IReportSale
    {
        public Task<object> GetDataReportSale(int companyId, int Year);
    }
}
