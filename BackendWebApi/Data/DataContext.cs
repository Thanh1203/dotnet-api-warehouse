using BackendWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendWebApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
        
        }

        public DbSet<Admin_Account> Admin_Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Classify> Classifies { get; set;}
        public DbSet<Product_Info> Product_Infos { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Warehouse_Info> Warehouse_Infos { get;set; }
        public DbSet<Personnel> Personnels { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Warehouse_Data> Warehouse_Data { get; set; }
        public DbSet<Warehouse_Import> Warehouse_Imports { get; set; }
        public DbSet<Warehouse_Export> Warehouse_Exports { get; set; }
        public DbSet<WH_Export_Data> WH_EX_Datas { get; set; }
        public DbSet<WH_Import_Data> WH_IM_Datas { get; set; }
    }
}
