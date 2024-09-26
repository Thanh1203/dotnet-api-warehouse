
namespace BackendWebApi.DTOS
{   
    public class DTOWarehouse_Update_UnitPrice
    {
        public int ProductId { get; set; }
        public double UntiPirce { get; set; }
    }
    public class DTOWarehouseData_DataInsert
    {
        public int IdProduct { get; set; }
        public string? CodeProduct { get; set; }
        public int Quantity { get; set; }
    }
    public class DTOWarehouseData_Create
    {
        public int IdWarehouse { get; set; }
        public int TotalProduct { get; set; }
        public List<DTOWarehouseData_DataInsert> DataInsert { get; set; }
    }
    public class DTOWarehouseData_DataUpdate
    {
        public int IdProduct { get; set; }
        public int Quantity { get; set; }
    }

    public class DTOWarehouseData_Update
    {
        public int IdWarehouse { get; set;}
        public int TotalProduct { get; set; }
        public List<DTOWarehouseData_DataUpdate> DataUpdate { get; set; }
    }

    public class DTOWh_Cus
    {
        public DTOWarehouseData_Update DataUpdate { get; set; }
        public DTOCustomer Customer { get; set; }
    }
}
