using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThoBayMau_ASM.Models
{
	[Table("LOAI_SP")]
	public class LoaiSP
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[Column(TypeName = "NVARCHAR(100)")]
		public string Ten { get; set; }
		public bool TrangThai { get; set; }
		public ICollection<SanPham> SanPhams { get; set; }
	}
}
