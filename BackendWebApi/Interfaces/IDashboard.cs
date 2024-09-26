namespace BackendWebApi.Interfaces
{
    public interface IDashboard
    {
       Task<object> GetInfoDashboard(int companyId);
    }
}
