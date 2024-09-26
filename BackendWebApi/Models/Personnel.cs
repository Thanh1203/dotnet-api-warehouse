using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BackendWebApi.Models
{
    [Table("Personnel")]
    public class Personnel
    {
        [Key]
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; }
        public string? Address { get; set; }
        [ForeignKey("Admin_Account")]
        [JsonIgnore]
        public int CompanyId { get; set; }
        [JsonIgnore]
        public ICollection<Warehouse_Info>? Warehouse_Info { get; set; }
        [JsonIgnore]
        public virtual Admin_Account? Admin_Account { get; set; }
        [NotMapped]
        public bool? AllowDelete {  get; set; }
    }
}
