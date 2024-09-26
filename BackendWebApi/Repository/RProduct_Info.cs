using BackendWebApi.Data;
using BackendWebApi.Interfaces;
using BackendWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendWebApi.DTOS;

namespace BackendWebApi.Repository
{
    public class RProduct_Info(DataContext context) : IProduct_Info
    {
        private readonly DataContext _context = context;

        public async Task<object> GetProduct_Infos(int companyid)
        {
            var data = new List<DTOProduct>();
            var dataList = await _context.Product_Infos.Where(e => e.CompanyId == companyid).OrderByDescending(e => e.DateTime).ToListAsync();
            var totalElement = await _context.Product_Infos.Where(e => e.CompanyId == companyid).CountAsync();
            
            foreach (var item in dataList)
            {
                bool allowDelete = await _context.Warehouse_Data.AnyAsync(p => p.IdProduct == item.Id || (p.Quantity > 0 && p.IdProduct == item.Id));
                string? categoryName = await _context.Categories.Where(p => p.Id == item.CategoryId).Select(item => item.Name).SingleOrDefaultAsync();
                string? classifyName = await _context.Classifies.Where(p => p.Id == item.ClassifyId).Select(item => item.Name).SingleOrDefaultAsync();
                string? producerName = await _context.Producers.Where(p => p.Id == item.ProducerId).Select(item => item.Name).SingleOrDefaultAsync();

                var viewModel = new DTOProduct
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                    CategoryId = item.CategoryId,
                    ClassifyId = item.ClassifyId,
                    ProducerId = item.ProducerId,
                    Size = item.Size,
                    Material = item.Material,
                    ConnectionTypes = item.ConnectionTypes,
                    Color = item.Color,
                    Designs = item.Designs,
                    Describe = item.Describe,
                    TimeCreate = TimeZoneInfo.ConvertTimeFromUtc(item.DateTime, TimeZoneInfo.Local).ToString("dd/MM/yyyy"),
                    AllowDelete = !allowDelete,
                    CategoryName = categoryName,
                    ClassifyName = classifyName,
                    ProducerName = producerName,
                    CompanyId = companyid
                };
                data.Add(viewModel);
            }

            return new
            {
                data,
                totalElement,
            }; ;
        }

        public async Task<object> SearchProductInfo(string strName, string categoryId, string classifyId, string producerId, int companyid)
        {
            var data = new List<DTOProduct>();
            var query = _context.Product_Infos.Where(e => e.CompanyId == companyid).OrderByDescending(e => e.DateTime).AsQueryable();

            if (!string.IsNullOrWhiteSpace(strName))
            {
                query = query.Where(e => e.Name.Contains(strName));
            }

            if (!string.IsNullOrWhiteSpace(categoryId))
            {
                query = query.Where(e => e.CategoryId == int.Parse(categoryId));
            }

            if (!string.IsNullOrWhiteSpace(classifyId))
            {
                query = query.Where(e => e.ClassifyId == int.Parse(classifyId));
            }

            if (!string.IsNullOrWhiteSpace(producerId))
            {
                query = query.Where(e => e.ProducerId == int.Parse(producerId));
            }
            var totalElement = await query.CountAsync();

            var dataList = await query.ToListAsync();
            foreach (var item in dataList)
            {
                bool allowDelete = await _context.Warehouse_Data.AnyAsync(p => p.IdProduct == item.Id ||(p.Quantity > 0 && p.IdProduct == item.Id));
                string? categoryName = await _context.Categories.Where(p => p.Id == item.CategoryId).Select(item => item.Name).SingleOrDefaultAsync();
                string? classifyName = await _context.Classifies.Where(p => p.Id == item.ClassifyId).Select(item=> item.Name).SingleOrDefaultAsync();
                string? producerName = await _context.Producers.Where(p => p.Id == item.ProducerId).Select(item => item.Name).SingleOrDefaultAsync();

                var viewModel = new DTOProduct
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                    CategoryId = item.CategoryId,
                    ClassifyId = item.ClassifyId,
                    ProducerId = item.ProducerId,
                    Size = item.Size,
                    Material = item.Material,
                    ConnectionTypes = item.ConnectionTypes,
                    Color = item.Color,
                    Designs = item.Designs,
                    Describe = item.Describe,
                    TimeCreate = TimeZoneInfo.ConvertTimeFromUtc(item.DateTime, TimeZoneInfo.Local).ToString("dd/MM/yyyy"),
                    AllowDelete = !allowDelete,
                    CategoryName = categoryName,
                    ClassifyName = classifyName,
                    ProducerName = producerName,
                    CompanyId = companyid,
                };
                data.Add(viewModel);
            }

            return new
            {
                data,
                totalElement,
            };
        }

        public async Task Create(Product_Info product, int companyid)
        {
            var newProduct = new Product_Info
            {
                Code = product.Code,
                Name = product.Name,
                CategoryId = product.CategoryId,
                ClassifyId = product.ClassifyId,
                ProducerId = product.ProducerId,
                Size = product.Size,
                Material = product.Material,
                ConnectionTypes = product.ConnectionTypes,
                Color = product.Color,
                Designs = product.Designs,
                Describe = product.Describe,
                DateTime = DateTime.UtcNow,
                CompanyId = companyid,
            };
            _context.Product_Infos.Add(newProduct);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Product_Info product, int companyid)
        {
            var temp = _context.Product_Infos.SingleOrDefault(e => e.Id == product.Id && e.CompanyId == companyid);
            if (temp != null)
            {
                temp.Name = product.Name;
                temp.CategoryId = product.CategoryId;
                temp.ClassifyId = product.ClassifyId;
                temp.ProducerId = product.ProducerId;
                temp.Size = product.Size;
                temp.Material = product.Material;
                temp.ConnectionTypes = product.ConnectionTypes;
                temp.Color = product.Color;
                temp.Designs = product.Designs;
                temp.Describe = product.Describe;
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(List<int> ids, int companyid)
        {
            foreach (var id in ids)
            {
                var itemDelete = _context.Product_Infos.SingleOrDefault(c => c.Id == id && c.CompanyId == companyid);

                if (itemDelete != null)
                {
                    _context.Product_Infos.Remove(itemDelete);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<object> GetProductOutsideWH(int warehouseId, int companyid)
        {
            var productInWh = await _context.Warehouse_Data.Where(e => e.CompanyId == companyid && e.IdWarehouse == warehouseId).Select(e => new { e.IdProduct, e.CodeProduct }).ToListAsync();
            var productData = await _context.Product_Infos.Where(e => e.CompanyId == companyid).Select(e => new { e.Id, e.Code }).ToListAsync();

            var dataList = productData.Where(p => !productInWh.Any(pwh => pwh.IdProduct == p.Id));

            var data = new List<DTOProducOutsideWH>();
            foreach (var item in dataList)
            {
                var temp = new DTOProducOutsideWH
                {
                    Id = item.Id + " | " + item.Code,
                    Code = item.Code,
                };
                data.Add(temp);
            }
            return data;
        }

        public async Task<object> GetProductInsideWH(int warehouseId, int companyid)
        {
            var dataList = await _context.Warehouse_Data.Where(e => e.CompanyId == companyid && e.IdWarehouse == warehouseId && e.UnitPrice > 0).ToListAsync();
            bool allHasUntiPrice = !await _context.Warehouse_Data.AnyAsync(e => e.CompanyId == 1 && e.IdWarehouse == warehouseId && e.UnitPrice == 0);
            var data = new List<DTOProductInWH>();
            foreach (var item in dataList)
            {
                string? NameProduct = await _context.Product_Infos.Where(e => e.Id == item.IdProduct).Select(e => e.Name).SingleOrDefaultAsync();
                var temp = new DTOProductInWH
                {
                    Id = item.IdProduct,
                    Code = item.CodeProduct,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Name = NameProduct
                };
                data.Add(temp);
            }
            return new 
            { 
                data,
                allHasUntiPrice
            };
        }

        public async Task<object> SearchProductInsideWH(int warehouseId, string code, int companyid)
        {
            var query =  _context.Warehouse_Data.Where(e => e.CompanyId == companyid && e.IdWarehouse == warehouseId && e.UnitPrice > 0).AsQueryable();
            bool allHasUntiPrice = !await _context.Warehouse_Data.AnyAsync(e => e.CompanyId == companyid && e.IdWarehouse == warehouseId && e.UnitPrice == 0);
            if (!string.IsNullOrWhiteSpace(code))
            {
                query = query.Where(e => e.CodeProduct.Contains(code));
            }
            var dataList = await query.ToListAsync();
            var data = new List<DTOProductInWH>();
            
            foreach(var item in dataList)
            {
                string? NameProduct = await _context.Product_Infos.Where(e => e.Id == item.IdProduct).Select(e => e.Name).SingleOrDefaultAsync();
                var temp = new DTOProductInWH
                {
                    Id = item.IdProduct,
                    Code = item.CodeProduct,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Name = NameProduct
                };
                data.Add(temp);
            }
            return new
            {
                data,
                allHasUntiPrice
            };
        }

        public async Task<object> GetProductConfigUnitPrice(int warehouseId, int companyid)
        {
            var dataList = await _context.Warehouse_Data.Where(e => e.CompanyId == companyid && e.IdWarehouse == warehouseId).OrderBy(item => item.UnitPrice == 0 ? 0 : 1).ToListAsync();
            var data = new List<DTOProductInWH>();
            foreach (var item in dataList)
            {
                string? NameProduct = await _context.Product_Infos.Where(e => e.Id == item.IdProduct).Select(e => e.Name).SingleOrDefaultAsync();
                var temp = new DTOProductInWH
                {
                    Id = item.IdProduct,
                    Code = item.CodeProduct,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Name = NameProduct
                };
                data.Add(temp);
            }
            return data;
        }

        public async Task<object> SearchProductConfigUnitPrice(int warehouseId, string code, string name, int companyid)
        {
            var query = _context.Warehouse_Data.Where(e => e.CompanyId == companyid && e.IdWarehouse == warehouseId).OrderBy(item => item.UnitPrice == 0 ? 0 : 1).AsQueryable();
            if (!string.IsNullOrWhiteSpace(code))
            {
                query = query.Where(e => e.CodeProduct.Contains(code));
            }
            var dataList = await query.ToListAsync();
            var datadto = new List<DTOProductInWH>();

            foreach (var item in dataList)
            {
                string? NameProduct = await _context.Product_Infos.Where(e => e.Id == item.IdProduct).Select(e => e.Name).SingleOrDefaultAsync();
                var temp = new DTOProductInWH
                {
                    Id = item.IdProduct,
                    Code = item.CodeProduct,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Name = NameProduct
                };
                datadto.Add(temp);
            }
            var data = datadto.AsQueryable();
            if (!string.IsNullOrWhiteSpace(name))
            {
                data = data.Where(e => e.Name.Contains(name));
            }

            return data;
        }
    }
}
