using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThoBayMau_ASM.Models
{
	[Table("TAI_KHOAN")]
	public class TaiKhoan
	{
		[Key]
		public int Id { get; set; }
		public string TenTK { get; set; }
		public string MakKhau { get; set; }
		public int SDT { get; set; }
		public string Email { get; set; }
		public string DiaChi { get; set; }
		public DateTime NgayDangKy { get; set; }
		public bool LoaiTK { get; set; }
		public bool TrangThai { get; set; }
		public ICollection<DonHang> DonHangs { get; set; }
		public ICollection<LichSu> LichSus { get; set; }
	}
}
