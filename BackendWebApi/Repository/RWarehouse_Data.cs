using BackendWebApi.Data;
using BackendWebApi.DTOS;
using BackendWebApi.Interfaces;
using BackendWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendWebApi.Repository
{
    public class RWarehouse_Data(DataContext context) : IWarehouse_Data
    {
        private readonly DataContext _context = context;

        public readonly string codeImport = "GsI" + 1.ToString() + GenerateRandomString() + DateTimeOffset.UtcNow.ToString("yyyyHHddMMfffmmss");
        public readonly string codeExport = "GsE" + 2.ToString() + GenerateRandomString() + DateTimeOffset.UtcNow.ToString("yyyyHHddMMfffmmss");
        public async Task UpdateUnitPrice(int idWarehouse, int productId, double unitPrice, int companyid)
        {
            var temp = _context.Warehouse_Data.SingleOrDefault(e => e.IdWarehouse== idWarehouse && e.IdProduct == productId && e.CompanyId == companyid);

            if (temp != null)
            {
                temp.UnitPrice = unitPrice;
            }

            await _context.SaveChangesAsync();
        }

        public async Task InsertProduct(DTOWarehouseData_Create? data_Create, int companyid)
        {
            foreach (var item in data_Create.DataInsert)
            {
                var newWarehouseData = new Warehouse_Data
                {
                    IdWarehouse = data_Create.IdWarehouse,
                    IdProduct = item.IdProduct,
                    CodeProduct = item.CodeProduct,
                    CompanyId = companyid,
                    Quantity = item.Quantity,
                    UnitPrice = 0,
                };
                _context.Warehouse_Data.Add(newWarehouseData);
                await _context.SaveChangesAsync();
            }

            var newWarehouseImport = new Warehouse_Import
            {

                WarehouseId = data_Create.IdWarehouse,
                TotalProduct = data_Create.TotalProduct,
                CompanyId = companyid,
                DateTime = DateTime.UtcNow,
                Code = codeImport
            };
            _context.Warehouse_Imports.Add(newWarehouseImport);
            await _context.SaveChangesAsync();

            foreach (var item in data_Create.DataInsert)
            {
                var importData = new WH_Import_Data
                {
                    Code = codeImport,
                    ProductId = item.IdProduct,
                    Quantity = item.Quantity,
                    DateTime = DateTime.UtcNow,
                    CompanyId = companyid
                };
                _context.WH_IM_Datas.Add(importData);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateQuantityProduct(DTOWarehouseData_Update? data_Update, int companyid)
        {
            foreach (var item in data_Update.DataUpdate)
            {
                var itemUpdate = _context.Warehouse_Data.SingleOrDefault(e => e.IdProduct == item.IdProduct && e.CompanyId == companyid && e.IdWarehouse == data_Update.IdWarehouse);
                if (itemUpdate != null)
                {
                    itemUpdate.Quantity += item.Quantity;
                }

                await _context.SaveChangesAsync();
            }

            var newWarehouseImport = new Warehouse_Import
            {

                WarehouseId = data_Update.IdWarehouse,
                TotalProduct = data_Update.TotalProduct,
                CompanyId = companyid,
                DateTime = DateTime.UtcNow,
                Code = codeImport
            };
            _context.Warehouse_Imports.Add(newWarehouseImport);
            await _context.SaveChangesAsync();

            foreach (var item in data_Update.DataUpdate)
            {
                var importData = new WH_Import_Data
                {
                    Code = codeImport,
                    ProductId = item.IdProduct,
                    Quantity = item.Quantity,
                    DateTime = DateTime.UtcNow,
                    CompanyId = companyid
                };
                _context.WH_IM_Datas.Add(importData);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DecreaseQuantityProduct(DTOWarehouseData_Update? data_Update, DTOCustomer? customer, int companyid)
        {
            double totalValue = 0;
            foreach (var item in data_Update.DataUpdate)
            {
                var itemUpdate = _context.Warehouse_Data.SingleOrDefault(e => e.IdProduct == item.IdProduct && e.CompanyId == companyid && e.IdWarehouse == data_Update.IdWarehouse);
                if (itemUpdate != null)
                {
                    itemUpdate.Quantity -= item.Quantity;
                    var value = item.Quantity * itemUpdate.UnitPrice;
                    totalValue += value;
                }

                await _context.SaveChangesAsync();
            }

            var newWarehouseExport = new Warehouse_Export
            {

                WarehouseId = data_Update.IdWarehouse,
                TotalProduct = data_Update.TotalProduct,
                CompanyId = companyid,
                DateTime = DateTime.UtcNow,
                Code = codeExport,
                TotalValue = totalValue,
            };
            _context.Warehouse_Exports.Add(newWarehouseExport);
            await _context.SaveChangesAsync();

            foreach (var item in data_Update.DataUpdate)
            {
                var exportData = new WH_Export_Data
                {
                    Code = codeExport,
                    ProductId = item.IdProduct,
                    Quantity = item.Quantity,
                    DateTime = DateTime.UtcNow,
                    CompanyId = companyid
                };

                _context.WH_EX_Datas.Add(exportData);
                await _context.SaveChangesAsync();
            }

            if (!string.IsNullOrWhiteSpace(customer.PhoneNumber))
            {
                var currentCustomer = await _context.Customers.SingleOrDefaultAsync(e => e.CompanyId == companyid && e.PhoneNumber.Contains(customer.PhoneNumber));
                if (currentCustomer != null)
                {
                    currentCustomer.TotalValueOrders += totalValue;
                    currentCustomer.PurchaseCount++;
                    currentCustomer.SalePoint += (totalValue >= 100000) ? (int)Math.Floor(totalValue / 1000 * 0.03) : 0;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var newCustomer = new Customer
                    {
                        PhoneNumber = customer.PhoneNumber,
                        CustomerName = customer.Name,
                        SalePoint = (totalValue >= 100000) ? (int)Math.Floor(totalValue / 1000 * 0.03) : 10,
                        PurchaseCount = 1,
                        Address = customer.Address,
                        DateTime = DateTime.UtcNow,
                        CompanyId = companyid,
                        TotalValueOrders = totalValue,
                    };
                    _context.Customers.Add(newCustomer);
                    await _context.SaveChangesAsync();
                }
            } 
        }

        static string GenerateRandomString()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            char[] result = new char[3];

            // Tạo chuỗi ngẫu nhiên có độ dài length ký tự
            for (int i = 0; i < 3; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }

            return new string(result);
        }
    }
}
