namespace QLBH_Dion.Models.ViewModel
{
    public class LoginViewModel
    {
        public int Id { get; set; } = 0;
        public int RoleId { get; set; } = 0;
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; } = "";
        public string FullName { get; set; } = "";
        public string RoleName { get; set; } = "";
        public int AccountStatusId { get; set; }
    }
}
