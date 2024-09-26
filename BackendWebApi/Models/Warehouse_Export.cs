using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BackendWebApi.Models
{
    [Table("Warehouse_Export")]
    public class Warehouse_Export
    {
        [Key]
        public string Code { get; set; }
        public int TotalProduct { get; set; }
        public DateTime DateTime { get; set; }
        public double TotalValue { get; set; }
        [JsonIgnore]
        [ForeignKey("Admin_Account")]
        public int CompanyId { get; set; }
        [JsonIgnore]
        [ForeignKey("Warehouse_Info")]
        public int WarehouseId { get; set; }
        [JsonIgnore]
        public virtual Admin_Account? Admin_Account { get; set; }
        [JsonIgnore]
        public virtual Warehouse_Info? Warehouse_Info { get; set; }
    }
}
