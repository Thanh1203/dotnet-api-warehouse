using BackendWebApi.DTOS;
using BackendWebApi.Helpers;
using BackendWebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackendWebApi.Controllers
{
    [Route("api/Login")]
    [ApiController]
    public class AuthController(IAdmin_Account account) :Controller
    {
        private readonly IAdmin_Account _IAdminAccount = account;
        
        [HttpPost("signUp")]
        public async Task<IActionResult> SignUpAdmin(DTOAdmin_Account_SignUp account_SignUp)
        {
            try
            {
                var result =  await _IAdminAccount.SignUp(account_SignUp);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost("signIn")]
        public async Task<IActionResult> SignInAdmin(DTOAdmin_Account_SignIn account_SignIn)
        {
            try
            {
                var result = await _IAdminAccount.SignIn(account_SignIn);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
