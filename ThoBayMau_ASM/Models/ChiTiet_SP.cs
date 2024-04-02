using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ThoBayMau_ASM.Validation;

namespace ThoBayMau_ASM.Models
{
	[Table("CHI_TIET_SP")]
	public class ChiTiet_SP
	{
		[Key]
		public int Id { get; set; }
        [Required(ErrorMessage = "Giá không được để trống")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Gía không được chứa ký tự đặc biệt")]
        [Range(minimum: 0, maximum: 2000000000, ErrorMessage = "giá không hợp lệ")]
        public int Gia { get; set; }
		[Required(ErrorMessage = "Số lượng không được để trống")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Số lượng không được chứa ký tự đặc biệt")]
        [Range(minimum: 0, maximum: 2000000000, ErrorMessage = "Số lượng không hợp lệ")]
        public int SoLuong { get; set; }
        [Required(ErrorMessage = "Số lượng không được để trống")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Số lượng không được chứa ký tự đặc biệt")]
        [Range(minimum: 0, maximum: 2000000000, ErrorMessage = "Số lượng không hợp lệ")]
        public int KichThuoc { get; set; }
        [Required(ErrorMessage = "Ngày sản xuất không được để trống")]
        public DateTime NgaySanXuat { get; set; }
        [Required(ErrorMessage = "Hạn sử dụng không được để trống")]
        public DateTime HanSuDung { get; set; }
		public int SanPhamId { get; set; }
		public SanPham? SanPham { get; set; }
		public bool TrangThai { get; set; }
	}
}
