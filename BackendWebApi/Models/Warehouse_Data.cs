using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BackendWebApi.Models
{
    [Table("WH_Data")]
    public class Warehouse_Data
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Warehouse_Info")]
        public int IdWarehouse { get; set; }
        [ForeignKey("Product_Info")]
        public int IdProduct { get; set; }
        public string CodeProduct { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        [ForeignKey("Admin_Account")]
        public int CompanyId { get; set; }
        [JsonIgnore]
        public virtual Admin_Account? Admin_Account { get; set; }
    }
}
