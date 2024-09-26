using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BackendWebApi.Models
{
    [Table("Producer")]
    public class Producer
    {
        [Key]
        public int Id { get; set; }
        public string Code {  get; set; }
        public string Name { get; set; }
        public string? Origin { get; set; }
        [ForeignKey("Admin_Account")]
        [JsonIgnore]
        public int CompanyId { get; set; }
        [JsonIgnore]
        public DateTime DateTime { get; set; }
        [JsonIgnore]
        public virtual ICollection<Product_Info>? Product_Info { get; set; }
        [JsonIgnore]
        public virtual Admin_Account? Admin_Account { get; set; }
        [NotMapped]
        public bool AllowDelete { get; set; }
    }
}
