using BackendWebApi.Data;
using BackendWebApi.DTOS;
using BackendWebApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackendWebApi.Repository
{
    public class RDashboard(DataContext context) : IDashboard
    {
        private readonly DataContext _context = context;

        public async Task<object> GetInfoDashboard(int companyid)
        {
            var totalProduct = await _context.Product_Infos.Where(e => e.CompanyId == companyid).CountAsync();
            var totalCustomer = await _context.Customers.Where(e => e.CompanyId == companyid).CountAsync();
            var totalWarehouse = await _context.Warehouse_Infos.Where(e => e.CompanyId == companyid).CountAsync();
            var secondBuy = await _context.Customers.Where(e => e.CompanyId == companyid && e.PurchaseCount >= 2).CountAsync();

            double rateReturn = 0;
            if (totalCustomer > 0)
            {
                rateReturn = (double)secondBuy / totalCustomer * 100;
            }

            var categoryRatios = await _context.Categories.Where(e => e.CompanyId == companyid)
                .Select(e => new
                {
                    e.Name,
                    Ratio = Math.Round(totalProduct == 0 ? 0 : _context.Product_Infos.Count(p => p.CategoryId == e.Id && p.CompanyId == companyid) / (double)totalProduct * 100, 1),
                }).ToListAsync();
            var dataCategoryRatio = new
            {
                Names = categoryRatios.Select(e => e.Name).ToArray(),
                Ratios = categoryRatios.Select(c => c.Ratio).ToArray()
            };

            var tottalPersonnal = await _context.Personnels.Where(e => e.CompanyId == companyid).CountAsync();

            int currentMonth = 1;
            var queryRevenue = await _context.Warehouse_Exports.Where(e => e.CompanyId == companyid).ToListAsync();
            var dataDtoRenenue = new List<DTODashboardRevenue>();
            foreach (var item in queryRevenue)
            {
                var viewModel = new DTODashboardRevenue
                {
                    Value = item.TotalValue,
                    Time = TimeZoneInfo.ConvertTimeFromUtc(item.DateTime, TimeZoneInfo.Local),
                };
                dataDtoRenenue.Add(viewModel);
            }
            List<double> dataRenenue = [];
            double totalRevenue = 0;
            while (currentMonth <= 12)
            {
                double temp = 0;
                foreach (var item in dataDtoRenenue)
                {
                    if (item.Time.Month == currentMonth)
                    {
                        temp += item.Value;
                    }
                }

                dataRenenue.Add(temp);
                totalRevenue += temp;
                currentMonth++;
            }

            return new
            {
                totalProduct,
                totalCustomer,
                totalWarehouse,
                rateReturn,
                dataCategoryRatio,
                tottalPersonnal,
                dataRenenue,
                totalRevenue,
            };
        }
    }
}
