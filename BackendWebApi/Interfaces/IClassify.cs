using BackendWebApi.Models;

namespace BackendWebApi.Interfaces
{
    public interface IClassify
    {
        Task<object> GetClassifies(int companyId);
        Task<object> SearchCalassifies(string name, int companyId);
        Task Create (Classify classify, int companyId);
        Task Update (Classify classify, int companyId);
        Task Delete (List<int> ids, int companyId);
    }
}
