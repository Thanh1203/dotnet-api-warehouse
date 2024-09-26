using BackendWebApi.Data;
using BackendWebApi.DTOS;
using BackendWebApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackendWebApi.Repository
{
    public class RReportSale(DataContext context) : IReportSale
    {
        private readonly DataContext _context = context;

        public static int GetDaysInYear(int year)
        {
            return DateTime.IsLeapYear(year) ? 366 : 365;
        }

        public async Task<object> GetDataReportSale(int companyId, int currentYear)
        {
            var queryCtn = await _context.Warehouse_Exports.Where(e => e.CompanyId == companyId).ToListAsync();
            var queryYearCtn = queryCtn.Where(e => e.DateTime.ToLocalTime().Year == currentYear).ToList();
            /// Tổng doanh thu
            var totalRevenue = queryYearCtn.Sum(e => e.TotalValue);
            /// Doanh thu trung bình theo ngày
            var averageDailyRevenue = Math.Round(totalRevenue == 0 ? 0 : totalRevenue/GetDaysInYear(currentYear),2);
            /// Doanh thu trung bình theo tháng
            var averageMonthlyRevenue = Math.Round(totalRevenue == 0 ? 0 : totalRevenue / 12, 2);
            /// Doanh thu theo quý
            List<double> quaterlyRevenue = [];
            int currentQuater = 1;
            while (currentQuater <= 10)
            {
                var temp = queryYearCtn.Where(e => e.DateTime.ToLocalTime().Month >= currentQuater && e.DateTime.ToLocalTime().Month <= currentQuater + 2).Sum(e => e.TotalValue);
                quaterlyRevenue.Add(temp);
                currentQuater += 3;
            }
            /// Doanh thu từng tháng
            List<double> monthRenenue = [];
            int currentMonth = 1;
            while (currentMonth <= 12)
            {
                var temp = queryYearCtn.Where(e => e.DateTime.ToLocalTime().Month == currentMonth).Sum(e => e.TotalValue);
                monthRenenue.Add(temp);
                currentMonth++;
            }
            /// Doanh thu theo khu vực
            List<DTOReportSaleArea> dataAreaReneue = [];
            var listArea = await _context.Warehouse_Infos.Where(e => e.CompanyId == companyId).Select(w => new {Area = w.Area, Id = w.Id}).ToListAsync();
            foreach (var item in listArea)
            {
                var itemSelect = queryYearCtn.Where(e => e.WarehouseId == item.Id).Sum(e => e.TotalValue);
                var viewdata = new DTOReportSaleArea
                {
                    Name = item.Area,
                    Value = itemSelect,
                };
                dataAreaReneue.Add(viewdata);
            }
            var areaReneue = dataAreaReneue.GroupBy(item => item.Name.ToLower()).Select(g => new
            {
                NormalizedName = g.Key,
                Value = g.Sum(g => g.Value),
            }).Select(item => new
            {
                name = dataAreaReneue.First(x => x.Name.ToLower() == item.NormalizedName).Name,
                value = item.Value,
            }).ToList();

            var areaWarehouseRenenue = new
            {
                Names = areaReneue.Select(e => e.name).ToArray(),
                TotalValue = areaReneue.Select(e => e.value).ToArray()
            };
            /// Doanh thu tưng kho hàng
            List<DTOReportSaleWarehouse> dataWarehouseRenenue = [];
            var listWarehouse = await _context.Warehouse_Infos.Where(e => e.CompanyId == companyId).Select(w => new {WhCode = w.Code, Id = w.Id}).ToListAsync();
            foreach (var item in listWarehouse)
            {
                var itemSelect = queryYearCtn.Where(e => e.WarehouseId == item.Id).Sum(e => e.TotalValue);
                var viewdata = new DTOReportSaleWarehouse
                {
                    Code = item.WhCode,
                    Value = itemSelect
                };
                dataWarehouseRenenue.Add(viewdata);
            }

            var warehouseRenenue = new
            {
                Codes = dataWarehouseRenenue.Select(e => e.Code).ToArray(),
                TotalValue = dataWarehouseRenenue.Select(e => e.Value).ToArray()
            };

            return new
            {
                totalRevenue,
                averageDailyRevenue,
                averageMonthlyRevenue,
                quaterlyRevenue,
                monthRenenue,
                areaWarehouseRenenue,
                warehouseRenenue
            };
        }
    }
}
