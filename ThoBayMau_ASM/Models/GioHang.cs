using Humanizer.Localisation.TimeToClockNotation;

namespace ThoBayMau_ASM.Models
{
	public class GioHang
	{
		public List<Giohang_Line>? Lines { get; set; } = new List<Giohang_Line>();
		public void AddItem(ChiTiet_SP ct_sp, int soluong)
		{
			Giohang_Line? line = Lines
				.Where(x => x.ChiTiet_SP.Id == ct_sp.Id)
				.FirstOrDefault();
			if (line == null)
			{
				Lines.Add(new Giohang_Line { ChiTiet_SP = ct_sp, SoLuong = soluong });
			}
			else
			{
				line.SoLuong += soluong;
			}
		}
		public void RemoveSanPham(int Id) => Lines.Remove(Lines.Where(p => p.ChiTiet_SP.Id == Id).FirstOrDefault());
		public int TamTinh() => (int)Lines.Sum(p => p.ChiTiet_SP.Gia * p.SoLuong);
		public int TongTien() => (int)(TamTinh() + 20);
		public void Clear() => Lines.Clear();

	}
	public class Giohang_Line
	{
		public int Giohang_Line_Id { get; set; }
		public ChiTiet_SP ChiTiet_SP { get; set; } = new();	
		public int SoLuong { get; set; }
		public int TongTienSp() => (int)(SoLuong * ChiTiet_SP.Gia);
	}


}
