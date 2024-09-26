using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BackendWebApi.Models
{
    [Table("Warehouse_Info")]
    public class Warehouse_Info {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Nation { get; set; }
        [ForeignKey("Personnel")]
        public int? StaffId { get; set; }
        public string Area { get; set; }
        public string? Address { get; set; }
        [ForeignKey("Admin_Account")]
        [JsonIgnore]
        public int CompanyId { get; set; }
        [JsonIgnore]
        public DateTime DateTime { get; set; }
        [JsonIgnore]
        public virtual Personnel? Personnel { get; set; }
        [JsonIgnore]
        public virtual Admin_Account? Admin_Account { get; set; }
        [NotMapped]
        public bool AllowDelete { get; set; }
        [JsonIgnore]
        public virtual ICollection<Warehouse_Import>? Warehouse_Imports { get; set; }
        [JsonIgnore]
        public virtual ICollection<Warehouse_Export>? Warehouse_Exports { get; set; }
    }
}
