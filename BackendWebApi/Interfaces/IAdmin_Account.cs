using BackendWebApi.DTOS;
using BackendWebApi.Models;

namespace BackendWebApi.Interfaces
{
    public interface IAdmin_Account
    {
        Task<object> SignUp(DTOAdmin_Account_SignUp account);
        Task<object> SignIn(DTOAdmin_Account_SignIn account);
    }
}
