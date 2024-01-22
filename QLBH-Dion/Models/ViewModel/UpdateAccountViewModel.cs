using System.ComponentModel.DataAnnotations;

namespace QLBH_Dion.Models.ViewModel
{
    public class UpdateAccountViewModel
    {
        [Required(ErrorMessage = "Trạng thái tài khoản không được để trống.")]
        public int AccountStatusId { get; set; }

        [Required(ErrorMessage = "Mã tài khoản không được để trống.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
        [StringLength(255, ErrorMessage = "Tên đăng nhập có độ dài tối đa là 255 ký tự.")]

        public string Username { get; set; } = null!;

        public string? Password { get; set; } = null!;


        [Required(ErrorMessage = "Họ tên không được để trống.")]
        [StringLength(255, ErrorMessage = "Họ tên có độ dài tối đa là 255 ký tự.")]

        public string FullName { get; set; }

        [Required(ErrorMessage = "Vai trò không được để trống.")]
        public int RoleId { get; set; }

        
    }
}
