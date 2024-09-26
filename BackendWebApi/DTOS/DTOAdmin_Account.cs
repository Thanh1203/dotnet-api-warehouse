namespace BackendWebApi.DTOS
{
    public class DTOAdmin_Account_SignUp
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class DTOAdmin_Account_SignIn
    {
        public string Username { get; set; }
        public string Password { get; set; }
        
    }
    public class DTOAdmin_Info
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
    }
}
