using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BackendWebApi.Models
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        public string? PhoneNumber { get; set; }
        public string? CustomerName { get; set; }
        public double SalePoint { get; set; }
        public int PurchaseCount { get; set; }
        public string? Address { get; set; }
        public double TotalValueOrders { get; set; }
        public DateTime DateTime { get; set; }
        [ForeignKey("Admin_Account")]
        public int CompanyId { get; set; }
        [JsonIgnore]
        public virtual Admin_Account? Admin_Account { get; set; }
    }
}
