using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThoBayMau_ASM.Models
{
	[Table("LICH_SU")]
	public class LichSu
	{
		[Key]
		public int Id { get; set; }
		public string ThongTin_ThaoTac { get; set; }
		public DateTime NgayGio {  get; set; }
		public string ChiTiet { get; set; }
		public int TaiKhoanId { get; set; }
		public TaiKhoan TaiKhoan { get; set; }
	}
}
