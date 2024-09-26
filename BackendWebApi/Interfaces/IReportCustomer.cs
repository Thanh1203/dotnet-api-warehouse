namespace BackendWebApi.Interfaces
{
    public interface IReportCustomer
    {
        public Task<object> FetchReport(int companyid);
        public Task<object> FetchReportNewCustomer(int currentyearm, int companyid);
    }
}
