using BackendWebApi.Data;
using BackendWebApi.DTOS;
using BackendWebApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackendWebApi.Repository
{
    public class RReportProduct(DataContext context) : IReportProduct
    {
        private readonly DataContext _context = context;

        public async Task<object> GetReportProduct(int companyId)
        {
            /// Tống số sản phẩm
            var totalProduct = await _context.Product_Infos.Where(e => e.CompanyId == companyId).CountAsync();
            /// Số lượng chủng loại
            var totalCategory = await _context.Categories.Where(e => e.CompanyId == companyId).CountAsync();
            /// Số lượng phân loại
            var totalClassifies = await _context.Classifies.Where(e => e.CompanyId == companyId).CountAsync();
            /// Số lượng hãng sản xuất
            var totalProducer = await _context.Producers.Where(e => e.CompanyId == companyId).CountAsync();
            /// Thống kê chủng loại
            var categoryRatios = await _context.Categories.Select(e => new
            {
                e.Name,
                Ratio = Math.Round(totalProduct == 0 ? 0 : _context.Product_Infos.Count(p => p.CategoryId == e.Id && p.CompanyId == companyId) / (double)totalProduct * 100, 1),
            }).ToListAsync();
            var dataCategoryRatio = new
            {
                Names = categoryRatios.Select(e => e.Name).ToArray(),
                Ratios = categoryRatios.Select(c => c.Ratio).ToArray()
            };
            /// Danh sách chủng loại
            var categoryList = await _context.Categories.Select(e => new
            {
                e.Name,
                Quantity = _context.Product_Infos.Count(p => p.CategoryId == e.Id && p.CompanyId == companyId),
            }).ToListAsync();
            /// Thống kê phân loại
            var classifyRatitos = await _context.Classifies.Select(e => new
            {
                e.Name,
                Ratio = Math.Round(totalProduct == 0 ? 0 : _context.Product_Infos.Count(p => p.ClassifyId == e.Id && p.CompanyId == companyId) / (double)totalProduct * 100, 1),
            }).ToListAsync();
            var dataClassifyRatio = new
            {
                Names = classifyRatitos.Select(e => e.Name).ToArray(),
                Ratios = classifyRatitos.Select(e => e.Ratio).ToArray()
            };
            /// Danh sách phân loại
            var classifyList = await _context.Classifies.Select(e => new
            {
                e.Name,
                Quantity = _context.Product_Infos.Count(p => p.ClassifyId == e.Id && p.CompanyId == companyId),
            }).ToListAsync();
            /// Thống kê hãng sản xuất
            var producerRatio = await _context.Producers.Select(e => new
            {
                e.Name,
                Ratio = Math.Round(totalProduct == 0 ? 0 : _context.Product_Infos.Count(p => p.ProducerId == e.Id && p.CompanyId == companyId) / (double)totalProduct * 100, 1),
            }).ToListAsync();
            var dataProducerRatio = new
            {
                Names = producerRatio.Select(e => e.Name).ToArray(),
                Ratios = producerRatio.Select(e => e.Ratio).ToArray()
            };
            /// Danh sách hãng sản xuất
            var producerList = await _context.Producers.Select(e => new
            {
                e.Name,
                Quantity = _context.Product_Infos.Count(p => p.ProducerId == e.Id && e.CompanyId == companyId),
            }).ToListAsync();

            return new
            {
                totalProduct,
                totalCategory,
                totalClassifies,
                totalProducer,
                dataCategoryRatio,
                categoryList,
                dataClassifyRatio,
                classifyList,
                dataProducerRatio,
                producerList,
            };
        }

        public async Task<object> GetReportSaleProduct(int companyId, int year, int? month)
        {
            var queryCtn = await  _context.WH_EX_Datas.Where(e => e.CompanyId == companyId).ToListAsync();

            if (month == null)
            {
                queryCtn = queryCtn.Where(e => e.DateTime.ToLocalTime().Year == year).ToList();
            }
            else
            {
                queryCtn = queryCtn.Where(e => e.DateTime.ToLocalTime().Year == year && e.DateTime.ToLocalTime().Month == month).ToList();
            }

            var productQuantities = queryCtn.GroupBy(e => e.ProductId).Select(g => new
            {
                ProductId = g.Key, 
                TotalQuantity = g.Sum(e => e.Quantity)
            }).AsQueryable();

            /// top sale
            var topSaleProduct = new List<DTOReportProduct>();
            var queryTopSale = productQuantities.OrderByDescending(p => p.TotalQuantity).Take(20).ToList();
            foreach (var item in queryTopSale)
            {
                string? nameProduct = await _context.Product_Infos.Where(e => e.Id == item.ProductId).Select(e => e.Name).SingleOrDefaultAsync();
                var viewTopSale = new DTOReportProduct
                {
                    Name = nameProduct,
                    Quantity = item.TotalQuantity
                };
                topSaleProduct.Add(viewTopSale);
            }
            
            /// excess inventory
            var excessInventory = new List<DTOReportProduct>(20);
            var queryProductNotSale = await _context.Product_Infos.Where(e => e.CompanyId == companyId).ToListAsync();
            if (month == null)
            {
                queryProductNotSale = queryProductNotSale.Where(e => e.CompanyId == companyId && e.DateTime.ToLocalTime().Year == year && !queryCtn.Any(p => p.ProductId == e.Id)).ToList();
            }
            else
            {
                queryProductNotSale = queryProductNotSale.Where(e => e.CompanyId == companyId && e.DateTime.ToLocalTime().Year == year && e.DateTime.ToLocalTime().Month == month && !queryCtn.Any(p => p.ProductId == e.Id)).ToList();
            }
            foreach (var item in queryProductNotSale)
            {
                var viewExcessInventory = new DTOReportProduct
                {
                    Name = item.Name,
                    Quantity = 0
                };
                excessInventory.Add(viewExcessInventory);
            }
            int remainingLength = 20 - excessInventory.Count;
            var queryExcessInventory = productQuantities.OrderBy(p => p.TotalQuantity).Take(remainingLength <= 0 ? 0 : remainingLength).ToList();
            if (queryExcessInventory.Count > 0)
            {
                foreach (var item in queryExcessInventory)
                {
                    string? nameProduct = await _context.Product_Infos.Where(e => e.Id == item.ProductId).Select(e => e.Name).SingleOrDefaultAsync();
                    var viewExcessInventory = new DTOReportProduct
                    {
                        Name = nameProduct,
                        Quantity = item.TotalQuantity
                    };
                    excessInventory.Add(viewExcessInventory);
                }
            }

            return new
            {
                topSaleProduct,
                excessInventory
            };
        }
    }
}
