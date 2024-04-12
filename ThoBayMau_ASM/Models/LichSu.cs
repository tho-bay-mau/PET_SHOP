using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThoBayMau_ASM.Models
{
	[Table("LICH_SU")]
	public class LichSu
	{
		[Key]
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Display(Name = "Thao tác")]
        public string ThongTin_ThaoTac { get; set; }
        [Display(Name = "Thời gian")]
        public DateTime NgayGio {  get; set; }
        [Display(Name = "Chi tiết")]
        public string ChiTiet { get; set; }
        [Display(Name = "ID tài khoản thao tác")]
        public int TaiKhoanId { get; set; }
		public TaiKhoan TaiKhoan { get; set; }
	}
}
