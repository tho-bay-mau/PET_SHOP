using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ThoBayMau_ASM.Validation;

namespace ThoBayMau_ASM.Models
{
	[Table("TAI_KHOAN")]
	public class TaiKhoan
	{
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "Varchar(50)")]
        [Display(Name = "Tên tài khoản")]
        [Required(ErrorMessage = "Tên tài khoản không được để trống")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Tên tài khoản không được chứa khoảng trắng hoặc ký tự tiếng Việt")]
        public string TenTK { get; set; }
        [Column(TypeName = "Varchar(50)")]
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [CheckWhiteSpace]
        [RegularExpression(@"^[a-zA-Z0-9\W]+$", ErrorMessage = "Mật khẩu không được chứa ký tự tiếng Việt")]
        public string MatKhau { get; set; }
        [Column(TypeName = "Varchar(11)")]
        [Required(ErrorMessage = "SDT không được để trống")]
        public string SDT { get; set; }
        [Column(TypeName = "Varchar(30)")]
        [Required(ErrorMessage = "Email không được để trống")]
        public string Email { get; set; }
        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }
        [Display(Name = "Ngày đăng ký")]
        public DateTime NgayDangKy { get; set; }
        [Display(Name = "Loại tài khoản")]
        public bool LoaiTK { get; set; }
        [Display(Name = "Trạng thái")]
        public bool TrangThai { get; set; }
        public ICollection<DonHang>? DonHangs { get; set; }
		public ICollection<LichSu>? LichSus { get; set; }
	}
}
