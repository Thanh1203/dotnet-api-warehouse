using BackendWebApi.Data;
using BackendWebApi.DTOS;
using BackendWebApi.Helpers;
using BackendWebApi.Interfaces;
using BackendWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace BackendWebApi.Repository
{
    public class RAdmin_Account(DataContext context, JwtService jwtService) : IAdmin_Account
    {
        private readonly DataContext _context = context;
        private readonly JwtService _jwtService = jwtService;
        public async Task<object> SignUp(DTOAdmin_Account_SignUp account)
        {
            var newAdminAccount = new Admin_Account
            {
                Name = account.Name,
                Phone = account.PhoneNumber,
                Email = account.Email,
                Password = account.Password,
            };
            _context.Admin_Accounts.Add(newAdminAccount);
            await _context.SaveChangesAsync();

            var accountData = await _context.Admin_Accounts.SingleOrDefaultAsync(e => e.Phone == account.PhoneNumber && e.Password == account.Password);

            if (accountData != null)
            {
                var JwtString = _jwtService.Generate(newAdminAccount.Id);
                var adminInfo = new DTOAdmin_Info
                {
                    CompanyId = newAdminAccount.Id,
                    Name = newAdminAccount.Name,
                    Email = newAdminAccount.Email,
                    PhoneNumber = newAdminAccount.Phone,
                    Token = JwtString,
                };

                return adminInfo;
            } else
            {
                return "sign up fail";
            }
        }

        public async Task<object> SignIn(DTOAdmin_Account_SignIn account)
        {
            var queryData = _context.Admin_Accounts.AsQueryable();
            Admin_Account accountData = null;
            if (IsValidEmail(account.Username))
            {
                accountData = queryData.SingleOrDefault(e => e.Email == account.Username && e.Password == account.Password);
            }
            else
            {
                accountData = queryData.SingleOrDefault(e => e.Phone == account.Username && e.Password == account.Password);
            }

           
            if (accountData == null) 
            {
                return "invalid account";
            }
            else
            {
                var JwtString = _jwtService.Generate(accountData.Id);
                var adminInfo = new DTOAdmin_Info
                {
                    CompanyId = accountData.Id,
                    Name = accountData.Name,
                    Email = accountData.Email,
                    PhoneNumber = accountData.Phone,
                    Token = JwtString,
                };

                GlobalConstant.CompanyId = accountData.Id;
                return adminInfo;
            }
        }

        public static bool IsValidEmail(string email)
        {
            // Biểu thức chính quy để kiểm tra địa chỉ email
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            // Tạo một đối tượng Regex với biểu thức chính quy
            Regex regex = new Regex(pattern);

            // Kiểm tra xem địa chỉ email khớp với biểu thức chính quy không
            return regex.IsMatch(email);
        }
    }
}
