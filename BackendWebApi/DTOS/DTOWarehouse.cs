namespace BackendWebApi.DTOS
{
    public class DTOWarehouse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Nation { get; set; }
        public int? StaffId { get; set; }
        public string Area { get; set; }
        public string Address { get; set; }
        public int CompanyId { get; set; }
        public bool AllowDelete { get; set; }
        public string? StaffName { get; set; }
        public string? TimeCreate { get; set; }
    }
}
