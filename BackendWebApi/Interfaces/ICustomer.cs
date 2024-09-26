using BackendWebApi.DTOS;
using BackendWebApi.Models;

namespace BackendWebApi.Interfaces
{
    public interface ICustomer
    {
        Task<object> SearchCustomer(string PhoneNumber, int companyId);
    }
}
