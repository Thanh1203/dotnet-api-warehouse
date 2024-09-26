using BackendWebApi.Data;
using BackendWebApi.Interfaces;
using BackendWebApi.Models;
using Microsoft.EntityFrameworkCore;
using BackendWebApi.DTOS;

namespace BackendWebApi.Repository
{
    public class RProducer(DataContext context) : IProducer
    {
        private readonly DataContext _context = context;

        public async Task<object> GetProducers(int companyid)
        {
            var data = new List<DTOProducer>();
            var totalElement = await _context.Producers.Where(e => e.CompanyId == companyid).CountAsync();
            var dataList = await _context.Producers.Where(e => e.CompanyId == companyid).OrderByDescending(e => e.DateTime).ToListAsync();

            foreach (var item in dataList)
            {
                bool allowDelete = await _context.Product_Infos.AnyAsync(p => p.ProducerId == item.Id);
                item.AllowDelete = !allowDelete;
                var viewModal = new DTOProducer
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                    Origin = item.Origin,
                    CompanyId = item.CompanyId,
                    TimeCreate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)item.DateTime, TimeZoneInfo.Local).ToString("dd/MM/yyyy"),
                    AllowDelete = item.AllowDelete,
                };
                data.Add(viewModal);
            }

            return new
            {
                data,
                totalElement,
            }; ;
        }

        public async Task<object> SearchProducer(string str, int companyid)
        {
            var data = new List<DTOProducer>();
            var query = _context.Producers.Where(e => e.CompanyId == companyid).OrderByDescending(e => e.DateTime).AsQueryable();

            if (!string.IsNullOrWhiteSpace(str))
            {
                query = query.Where(e => e.Name.Contains(str));
            }
           
            var dataList = await query.ToListAsync();

            foreach (var item in dataList)
            {
                bool allowDelete = await _context.Product_Infos.AnyAsync(p => p.ProducerId == item.Id);
                item.AllowDelete = !allowDelete;
                var viewModal = new DTOProducer
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                    Origin = item.Origin,
                    CompanyId = item.CompanyId,
                    TimeCreate = TimeZoneInfo.ConvertTimeFromUtc(item.DateTime, TimeZoneInfo.Local).ToString("dd/MM/yyyy"),
                };
                data.Add(viewModal);
            }

            var totalElement = await query.CountAsync();

            return new
            {
                data,
                totalElement,
            };
        }

        public async Task Create(Producer producer, int companyid)
        {
            var newProducer = new Producer
            {
                Code = producer.Code,
                Name = producer.Name,
                Origin = producer.Origin,
                DateTime = DateTime.UtcNow,
                CompanyId = companyid
            };
            _context.Producers.Add(newProducer);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Producer producer, int companyid)
        {
            var temp = _context.Producers.SingleOrDefault(e => e.Id == producer.Id && e.CompanyId == companyid);

            if (temp != null)
            {
                temp.Code = producer.Code;
                temp.Name = producer.Name;
                temp.Origin = producer.Origin;
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(List<int> ids, int companyid)
        {
            foreach (var id in ids)
            {
                var temp = _context.Producers.SingleOrDefault(e => e.Id == id && e.CompanyId == companyid);

                if (temp != null)
                {
                    _context.Producers.Remove(temp);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
