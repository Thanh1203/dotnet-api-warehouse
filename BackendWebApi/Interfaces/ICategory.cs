using BackendWebApi.Models;

namespace BackendWebApi.Interfaces
{
    public interface ICategory
    {
        Task<object> GetCategories(int companyId);
        Task<object> SearchCategory(string name, int companyId);
        Task Create (Category category, int companyId);
        Task Update (Category category, int companyId);
        Task Delete(List<int> ids, int companyId);
    }
}
