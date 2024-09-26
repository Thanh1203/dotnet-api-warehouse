using BackendWebApi.Data;
using BackendWebApi.Interfaces;
using BackendWebApi.Models;
using Microsoft.EntityFrameworkCore;
using BackendWebApi.DTOS;

namespace BackendWebApi.Repository
{
    public class RWarehouse_Info(DataContext context) : IWarehouse_Info
    {
        private readonly DataContext _context = context;

        public async Task<object> GetWarehouseInfos(int companyid)
        {
            var data = new List<DTOWarehouse>();
            var dataList = await _context.Warehouse_Infos.Where(e => e.CompanyId == companyid).ToListAsync();
            var totalElement = await _context.Warehouse_Infos.Where(e => e.CompanyId == companyid).CountAsync();

            foreach (var item in dataList)
            {
                bool allowDelete = await _context.Warehouse_Data.AnyAsync(p => p.Id == item.Id);
                string? staffName = await _context.Personnels.Where( p => p.Id == item.StaffId).Select(p => p.Name).SingleOrDefaultAsync();
                var viewModel = new DTOWarehouse
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                    Nation = item.Nation,
                    StaffId = item?.StaffId,
                    Area = item.Area,
                    Address = item.Address,
                    CompanyId = item.CompanyId,
                    AllowDelete = !allowDelete,
                    StaffName = staffName,
                    TimeCreate = TimeZoneInfo.ConvertTimeFromUtc(item.DateTime, TimeZoneInfo.Local).ToString("dd/MM/yyyy")
                };
                data.Add(viewModel);
            }

            return new
            {
                data,
                totalElement,
            };
        }

        public async Task<object> SearchWarehouseInfo(string strName, string strNation, string strArea, int companyid)
        {
            var data = new List<DTOWarehouse>();
            var query = _context.Warehouse_Infos.Where(e => e.CompanyId == companyid).AsQueryable();

            if (!string.IsNullOrWhiteSpace(strName))
            {
                query = query.Where(e => e.Name.Contains(strName));
            }

            if (!string.IsNullOrWhiteSpace(strNation))
            {
                query = query.Where(e => e.Nation.Contains(strNation));
            }

            if (!string.IsNullOrWhiteSpace(strArea))
            {
                query = query.Where(e => e.Area.Contains(strArea));
            }

            var dataList = await query.ToListAsync();

            foreach (var item in dataList)
            {
                bool allowDelete = await _context.Warehouse_Data.AnyAsync(p => p.Id == item.Id);
                string? staffName = await _context.Personnels.Where(p => p.Id == item.StaffId).Select(p => p.Name).SingleOrDefaultAsync();
                var viewModel = new DTOWarehouse
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                    Nation = item.Nation,
                    StaffId = item.StaffId,
                    Area = item.Area,
                    Address = item.Address,
                    CompanyId = item.CompanyId,
                    AllowDelete = !allowDelete,
                    StaffName = staffName,
                    TimeCreate = TimeZoneInfo.ConvertTimeFromUtc(item.DateTime, TimeZoneInfo.Local).ToString("dd/MM/yyyy")
                };
                data.Add(viewModel);
            }

            var totalElement = await query.CountAsync();


            return new
            {
                data,
                totalElement,
            };
        }

        public async Task<List<string>> GetNationWarehouse(int companyid)
        {
            return await _context.Warehouse_Infos.Where(e => e.CompanyId == companyid).Select(w => w.Nation).Distinct().ToListAsync();
        }

        public async Task<List<string>> GetAreaWarehouse(int companyid)
        {
            return await _context.Warehouse_Infos.Where(e => e.CompanyId == companyid).Select(w => w.Area).Distinct().ToListAsync();
        }

        public async Task Create(Warehouse_Info warehouse, int companyid)
        {
            var newWH = new Warehouse_Info
            {
                Code = warehouse.Code,
                Name = warehouse.Name,
                Nation = warehouse.Nation,
                Area = warehouse.Area,
                StaffId = warehouse.StaffId,
                Address = warehouse.Address,
                CompanyId = 1,
                DateTime = DateTime.UtcNow,

            };
            _context.Warehouse_Infos.Add(newWH);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Warehouse_Info warehouse, int companyid)
        {
            var temp = _context.Warehouse_Infos.SingleOrDefault(e => e.Id == warehouse.Id && e.CompanyId == companyid);
            
            if (temp != null)
            {
                temp.Name = warehouse.Name;
                temp.Nation = warehouse.Nation;
                temp.Area = warehouse.Area;
                temp.StaffId = warehouse.StaffId;
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(List<int> ids, int companyid)
        {
            foreach (int id in ids)
            {
                var temp = _context.Warehouse_Infos.SingleOrDefault(c => c.Id == id && c.CompanyId == companyid);

                if (temp != null)
                {
                    _context.Warehouse_Infos.Remove(temp);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
