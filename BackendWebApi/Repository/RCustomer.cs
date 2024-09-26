using BackendWebApi.Data;
using BackendWebApi.DTOS;
using BackendWebApi.Interfaces;
using BackendWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendWebApi.Repository
{
    public class RCustomer(DataContext context) : ICustomer
    {
        private readonly DataContext _context = context;

        public async Task<object> SearchCustomer(string phoneNumber, int companyId)
        {
            var data = new List<DTOCustomer>();
            var dataList = await _context.Customers.Where(e => e.CompanyId == companyId && e.PhoneNumber.Contains(phoneNumber)).ToListAsync();

            foreach (var item in dataList)
            {
                var viewModel = new DTOCustomer
                {
                    PhoneNumber = item.PhoneNumber,
                    Name = item.CustomerName,
                    Address = item.Address
                };
                data.Add(viewModel);
            }

            return data;
        }
    }
}