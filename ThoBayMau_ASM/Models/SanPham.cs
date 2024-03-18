using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThoBayMau_ASM.Models
{
	[Table("SAN_PHAM")]
	public class SanPham
	{
		[Key]
		public int Id { get; set; }
		public string Ten { get; set; }
		[Column(TypeName = "ntext")]
		public string? Mota { get; set; }
		[Column(TypeName = "nvarchar(50)")]
		public bool TrangThai { get; set; }
		public int LoaiSPId { get; set; }
		public LoaiSP? LoaiSP { get; set; }
		public ICollection<ChiTiet_SP>? ChiTietSPs { get; set; }
		public ICollection<Anh>? Anhs { get; set; }
	}
}
