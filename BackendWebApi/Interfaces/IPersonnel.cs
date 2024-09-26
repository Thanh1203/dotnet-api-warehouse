using BackendWebApi.Models;

namespace BackendWebApi.Interfaces
{
    public interface IPersonnel
    {
        Task<object> GetPersonnels(int companyId);
        Task<object> SearchPersonnel(string code, string name, string address, int companyId);
        Task Create(Personnel personnel, int companyId);
        Task Update(Personnel personnel, int companyId);
        Task Delete(List<int> ids, int companyId);
        Task<List<string>> GetAddressPersonnel(int companyId);
    }
}
