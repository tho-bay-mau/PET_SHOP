using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ThoBayMau_ASM.Models.ViewModel
{
    public class SanPhamViewModel
    {
        [Key]
        public int Id { get; set; }
        public string Ten { get; set; }
        [Column(TypeName = "ntext")]
        public string? Mota { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string TrangThai { get; set; }
        public int LoaiSPId { get; set; }
        public List<IFormFile> TenAnh { get; set; }
        public SanPham SanPham { get; set; }

    }
}
