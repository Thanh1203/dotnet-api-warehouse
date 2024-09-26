using BackendWebApi.Models;

namespace BackendWebApi.Interfaces
{
    public interface IProducer
    {
        Task<object> GetProducers(int companyId);
        Task<object> SearchProducer(string name, int companyId);
        Task Create (Producer producer, int companyId);
        Task Update (Producer producer, int companyId);
        Task Delete (List<int> ids, int companyId);
    }
}
