using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThoBayMau_ASM.Models
{
	[Table("DONHANG_CHITIET")]
	public class DonHang_ChiTiet
	{
		[Key]
		public int Id { get; set; }
		public int SoLuong { get; set; }
		public int DonHangId { get; set; }
		public DonHang DonHang { get; set; }
		public int SanPhamId { get; set; }
		public SanPham SanPham { get; set; }
	}
}
