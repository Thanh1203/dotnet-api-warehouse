namespace BackendWebApi.DTOS
{

    public class DTOReportCustomer
    {
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int PurchaseCount { get; set; }
        public double TotalOrders { get; set; }
    }
}
