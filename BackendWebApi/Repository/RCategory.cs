using BackendWebApi.Data;
using BackendWebApi.DTOS;
using BackendWebApi.Interfaces;
using BackendWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendWebApi.Repository
{
    public class RCategory(DataContext context) : ICategory
    {
        private readonly DataContext _context = context;

        public async Task<object> GetCategories(int companyid)
        {
            var data = new List<DTOCategory>();
            var datalist = await _context.Categories.Where(e => e.CompanyId == companyid).OrderByDescending(e=> e.DateTime).ToListAsync();
            var totalElement = await _context.Categories.Where(e => e.CompanyId == companyid).CountAsync();

            foreach (var item in datalist)
            {
                bool allowDelete = await _context.Product_Infos.AnyAsync(p => p.CategoryId == item.Id);
                var viewModel = new DTOCategory
                {
                    Id = item.Id,
                    Name = item.Name,
                    Code = item.Code,
                    CompanyId = item.CompanyId,
                    TimeCreate = TimeZoneInfo.ConvertTimeFromUtc(item.DateTime, TimeZoneInfo.Local).ToString("dd/MM/yyyy"),
                    AllowDelete = !allowDelete,
                };
                data.Add(viewModel);
            }

            return new 
            { 
                data,
                totalElement,
            };
        }

        public async Task<object> SearchCategory(string str, int companyId)
        {
            var data = new List<DTOCategory>();
            var query = _context.Categories.Where(e => e.CompanyId == companyId).OrderByDescending(e => e.DateTime).AsQueryable();

            if (!string.IsNullOrWhiteSpace(str))
            {
                query = query.Where(e => e.Name.Contains(str));
            }

            var totalElement = await query.CountAsync();
            var dataList = await query.ToListAsync();

            foreach (var item in dataList)
            {
                bool allowDelete = await _context.Product_Infos.AnyAsync(p => p.CategoryId == item.Id);
                var viewModel = new DTOCategory
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                    CompanyId = item.CompanyId,
                    TimeCreate = TimeZoneInfo.ConvertTimeFromUtc(item.DateTime, TimeZoneInfo.Local).ToString("dd/MM/yyyy"),
                    AllowDelete = !allowDelete,
                };
                data.Add(viewModel);
            }

            return new
            {
                data,
                totalElement,
            };
        }

        public async Task Create(Category category, int companyId)
        {
            var newCategory = new Category
            {
                Code = category.Code,
                Name = category.Name,
                DateTime = DateTime.UtcNow,
                CompanyId = companyId,
            };

            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Category category, int companyId)
        {
           var temp = _context.Categories.SingleOrDefault(e => e.Id == category.Id && e.CompanyId == companyId);

            if (temp != null)
            {
                temp.Code = category.Code;
                temp.Name = category.Name;
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(List<int> ids, int companyId)
        {
            foreach (var id in ids)
            {
                var temp = _context.Categories.SingleOrDefault(e => e.Id == id && e.CompanyId == companyId);

                if (temp != null)
                {
                    _context.Categories.Remove(temp);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
