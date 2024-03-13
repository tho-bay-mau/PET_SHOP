using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThoBayMau_ASM.Models
{
	[Table("CHI_TIET_SP")]
	public class ChiTiet_SP
	{
		[Key]
		public int Id { get; set; }
		public int Gia { get; set; }
		public int SoLuong { get; set; }
		public int KichThuoc { get; set; }
		public DateTime NgaySanXuat { get; set; }
		public DateTime HanSuDung { get; set; }
		public int SanPhamId { get; set; }
		public SanPham SanPham { get; set; }
	}
}
