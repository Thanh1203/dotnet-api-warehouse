namespace BackendWebApi.DTOS
{
    public class DTOProduct
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int CategoryId { get; set; }
        public int ClassifyId { get; set; }
        public int ProducerId { get; set; }
        public string? Size { get; set; }
        public string? Material { get; set; }
        public string? ConnectionTypes { get; set; }
        public string? Color { get; set; }
        public string? Designs { get; set; }
        public string? Describe { get; set; }
        public string? TimeCreate { get; set; }
        public int CompanyId { get; set; }
        public bool? AllowDelete { get; set; }
        public string? CategoryName { get; set; }
        public string? ClassifyName { get; set; }
        public string? ProducerName { get; set; }
    }

    public class DTOProducOutsideWH
    {
        public string? Id { get; set; }
        public string? Code { get; set; }
    }

    public class DTOProductInWH
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public int? Quantity { get; set; }
        public double UnitPrice { get; set; }
        public string? Name { get; set; }
    }
}
