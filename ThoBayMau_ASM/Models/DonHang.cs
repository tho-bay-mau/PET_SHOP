using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThoBayMau_ASM.Models
{
	[Table("DON_HANG")]
	public class DonHang
	{
		[Key]
		public int Id { get; set; }
		public DateTime? ThoiGianTao { get; set; } = DateTime.Now;
		public bool? TrangThaiThanhToan { get; set; }
		[Required]
		public string? TrangThaiDonHang { get; set; }
		public int TaiKhoanId { get; set; }
		public TaiKhoan TaiKhoan { get; set; }
		public ICollection<DonHang_ChiTiet>? DonHang_ChiTiets { get; set; }
	}
}
