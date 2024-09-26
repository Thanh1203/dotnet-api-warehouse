using BackendWebApi.Data;
using BackendWebApi.DTOS;
using BackendWebApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackendWebApi.Repository
{
    public class RWarehouse_Import(DataContext context) : IWarehouse_Import
    {
        private readonly DataContext _context = context;

        public async Task<object> GetHistoryImport(int warehouseId, int companyid)
        {
            var data = new List<DTOWarehouse_Import>();
            var dataList = await _context.Warehouse_Imports.Where(e => e.CompanyId == companyid && e.WarehouseId == warehouseId).OrderByDescending(e => e.DateTime).ToListAsync();
            foreach (var item in dataList)
            {
                var viewModel = new DTOWarehouse_Import
                {
                    Code = item.Code,
                    TotalProduct = item.TotalProduct,
                    TimeCreate = TimeZoneInfo.ConvertTimeFromUtc(item.DateTime, TimeZoneInfo.Local).ToString("dd/MM/yyyy"),
                };
                data.Add(viewModel);
            }

            return data;
        }

        public async Task<object> SearHistoryImport(string day, string month, string year, int warehouseId, int companyid)
        {
            var query = _context.Warehouse_Imports.Where(e => e.CompanyId == companyid && e.WarehouseId == warehouseId).OrderByDescending(e => e.DateTime).AsQueryable();
            var datadto = new List<DTOWarehouse_Import>();
            var dataList = await query.ToListAsync();

            foreach (var item in dataList)
            {
                var viewModel = new DTOWarehouse_Import
                {
                    Code = item.Code,
                    TotalProduct = item.TotalProduct,
                    TimeCreate = TimeZoneInfo.ConvertTimeFromUtc(item.DateTime, TimeZoneInfo.Local).ToString("dd/MM/yyyy"),
                };
                datadto.Add(viewModel);
            }

            var data = datadto.AsQueryable();
            if (!string.IsNullOrWhiteSpace(day))
            {
                data = data.Where(e => e.TimeCreate.Contains(day));
            }

            if (!string.IsNullOrWhiteSpace(month))
            {
                data = data.Where(e => e.TimeCreate.Contains(month));
            }

            if (!string.IsNullOrWhiteSpace(year))
            {
                data = data.Where(e => e.TimeCreate.Contains(year));
            }

            return data;
        }
    }
}
