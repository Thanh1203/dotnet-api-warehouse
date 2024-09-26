using BackendWebApi.Data;
using BackendWebApi.DTOS;
using BackendWebApi.Interfaces;
using BackendWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendWebApi.Repository
{
    public class RReportCustomer(DataContext context) : IReportCustomer
    {
        private readonly DataContext _context = context;
        public int currentYear = DateTime.Now.Year;
        public async Task<object> FetchReport(int companyid)
        {
            var tempQueryList = await _context.Customers.Where(e => e.CompanyId == companyid).ToListAsync();
            var tempQueryable =  _context.Customers.Where(e => e.CompanyId == companyid).AsQueryable();
            /// Số lượng khách hàng
            var totalCustomer = await tempQueryable.CountAsync();
            /// Số lượt mua hàng
            int totalPurchase = tempQueryable.Sum(e => e.PurchaseCount);
            /// Tỉ lệ khách quay lại
            var secondBuy = await tempQueryable.Where(e => e.PurchaseCount >= 2).CountAsync();
            double rateReturn = 0;
            if (totalCustomer > 0)
            {
                rateReturn = (double)secondBuy / totalCustomer * 100;
            }
            /// Số lượng khách hàng mới 5 năm đổ lại
            List<int> dataNewCustomerYear = [];
            int yearPass = currentYear - 5;
            while (yearPass <= currentYear)
            {
                int item = tempQueryList.Where(e => e.DateTime.ToLocalTime().Year == yearPass).Count();
                dataNewCustomerYear.Add(item);
                yearPass++;
            }
            /// Tỉ lệ số lần mua hàng trên số khách
            List<double> dataRatioPurchase = [];
            int countRatioPurchase = 1;
            while (countRatioPurchase <=3)
            {
                double buyTime = 0;
                if (countRatioPurchase == 3)
                {
                    buyTime = await tempQueryable.Where(e => e.PurchaseCount >= 3).CountAsync();
                }
                else
                {
                    buyTime = await tempQueryable.Where(e => e.PurchaseCount == countRatioPurchase).CountAsync();
                }
                double ratioBuyOneTime = Math.Round(buyTime / totalCustomer * 100, 1);
                dataRatioPurchase.Add(ratioBuyOneTime);
                countRatioPurchase++;
            }
            /// Danh sách khách hàng tiềm năng
            var dataPotentialCustomers = new List<DTOReportCustomer>();
            var listPotentialCustomers = await tempQueryable.OrderByDescending(e => e.TotalValueOrders).Take(20).ToListAsync();
            foreach ( var item in listPotentialCustomers)
            {
                var viewModel = new DTOReportCustomer
                {
                    PhoneNumber = item.PhoneNumber,
                    Name = item.CustomerName,
                    Address = item.Address,
                    PurchaseCount = item.PurchaseCount,
                    TotalOrders = item.TotalValueOrders,
                };
                dataPotentialCustomers.Add(viewModel);
            }

            return new
            {
                totalCustomer,
                totalPurchase,
                rateReturn,
                dataNewCustomerYear,
                dataRatioPurchase,
                dataPotentialCustomers
            };
        }

        public async Task<object> FetchReportNewCustomer(int year, int companyid)
        {
            var tempQueryList = await _context.Customers.Where(e => e.CompanyId == companyid).ToListAsync();
            /// Số lượng khách hàng mới theo tháng trong năm

            int totalNewCustomer = 0;
            List<int> dataNewCustomer = [];
            int currentMonth = 1;
            while (currentMonth <= 12)
            {
                var item = tempQueryList.Where(e => e.DateTime.ToLocalTime().Month == currentMonth && e.DateTime.ToLocalTime().Year == year).Count();
                totalNewCustomer += item;
                dataNewCustomer.Add(item);
                currentMonth++;
            }

            return new
            {
                totalNewCustomer,
                dataNewCustomer
            };
        }
    }
}
