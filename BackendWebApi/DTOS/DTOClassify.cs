namespace BackendWebApi.DTOS
{
    public class DTOClassify
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public string? TimeCreate { get; set; }
        public bool? AllowDelete { get; set; }
    }
}
