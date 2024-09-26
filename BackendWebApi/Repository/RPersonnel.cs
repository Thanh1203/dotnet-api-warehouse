using BackendWebApi.Data;
using BackendWebApi.Interfaces;
using BackendWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendWebApi.Repository
{
    public class RPersonnel(DataContext context) : IPersonnel
    {
        private readonly DataContext _context = context;

        public async Task<object> GetPersonnels(int companyid)
        {
            var totalElement = await _context.Personnels.Where(e => e.CompanyId == companyid).CountAsync();
            var data = await _context.Personnels.Where(e => e.CompanyId == companyid).ToListAsync();

            foreach (var item in data)
            {
                bool allowDelete = await _context.Warehouse_Infos.AnyAsync(p => p.StaffId == item.Id);
                item.AllowDelete = !allowDelete;
            };

            var result = new
            {
                data,
                totalElement,
            };

            return result;
        }

        public async Task<object> SearchPersonnel(string strCode, string strName, string strAddress, int companyid)
        {
            var query = _context.Personnels.Where(e => e.CompanyId == companyid).AsQueryable();

            if (!string.IsNullOrWhiteSpace(strCode))
            {
                query = query.Where(e => e.Code.Contains(strCode));
            }

            if (!string.IsNullOrWhiteSpace(strName))
            {
                query = query.Where(e => e.Name.Contains(strName));
            }

            if (!string.IsNullOrWhiteSpace(strAddress))
            {
                query = query.Where(e => e.Address.Contains(strAddress));
            }

            var data = await query.ToListAsync();

            foreach (var item in data)
            {
                bool allowDelete = await _context.Warehouse_Infos.AnyAsync(p => p.StaffId == item.Id);

                item.AllowDelete = !allowDelete;
            }

            var totalElement = await query.CountAsync();

            var result = new
            {
                data,
                totalElement,
            };

            return result;
        }

        public async Task Create(Personnel personnel, int companyid)
        {
            var newPersonnel = new Personnel
            {
                Code = personnel.Code,
                Name = personnel.Name,
                Address = personnel.Address,
                Role = personnel.Role,
                CompanyId = companyid,
            };

            _context.Personnels.Add(newPersonnel);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Personnel personnel, int companyid)
        {
            var temp = _context.Personnels.SingleOrDefault(p => p.Id == personnel.Id && p.CompanyId == companyid);

            if (temp != null)
            {
                temp.Name = personnel.Name;
                temp.Address = personnel.Address;
                temp.Role = personnel.Role;
                temp.Warehouse_Info = personnel.Warehouse_Info;
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(List<int> ids, int companyid)
        {
            foreach (var id in ids)
            {
                var temp = _context.Personnels.SingleOrDefault(e => e.Id == id && e.CompanyId == companyid);

                if (temp != null)
                {
                    _context.Personnels.Remove(temp);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<List<string>> GetAddressPersonnel(int companyid) => await _context.Personnels.Where(e => e.CompanyId == companyid).Select(e => e.Address).Distinct().ToListAsync();
    }
}
