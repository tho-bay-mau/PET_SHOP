using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThoBayMau_ASM.Models
{
	[Table("ANH")]
	public class Anh
	{
		[Key]
		public int Id { get; set; }
		public string TenAnh { get; set; }
		public int SanphamId { get; set; }
		public SanPham SanPham { get; set; }
	}
}
