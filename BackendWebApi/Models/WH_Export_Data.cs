using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BackendWebApi.Models
{
    [Table("WH_Export_Data")]
    public class WH_Export_Data
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Warehouse_Export")]
        public string Code { get; set; }
        [ForeignKey("Product_Info")]
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime DateTime { get; set; }
        [ForeignKey("Admin_Account")]
        public int CompanyId { get; set; }
        [JsonIgnore]
        public virtual Admin_Account? Admin_Account { get; set; }

    }
}
