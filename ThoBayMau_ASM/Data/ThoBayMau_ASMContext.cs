﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ThoBayMau_ASM.Models;

namespace ThoBayMau_ASM.Data
{
    public class ThoBayMau_ASMContext : DbContext
    {
        public ThoBayMau_ASMContext (DbContextOptions<ThoBayMau_ASMContext> options): base(options)
        {
        }

        public DbSet<ThoBayMau_ASM.Models.SanPham> SanPham { get; set; } = default!;
        public DbSet<ThoBayMau_ASM.Models.DonHang> DonHang { get; set; } = default!;
        public DbSet<ThoBayMau_ASM.Models.TaiKhoan> TaiKhoan { get; set; } = default!;
        public DbSet<ThoBayMau_ASM.Models.LoaiSP> LoaiSP { get; set; } = default!;
        public DbSet<ThoBayMau_ASM.Models.ChiTiet_SP> ChiTiet_SP { get; set; } = default!;
        public DbSet<ThoBayMau_ASM.Models.Anh> Anh { get; set; } = default!;
        public DbSet<ThoBayMau_ASM.Models.LichSu> LichSu { get; set; } = default!;
        public DbSet<ThongKeDoanhThu> ThongKeDoanhThu { get; set; }
        public DbSet<SPTop5> SPTop5 { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ThongKeDoanhThu>().HasNoKey();
            modelBuilder.Entity<SPTop5>().HasNoKey();
        }
    }
    public class ThongKeDoanhThu
    {
        public int Thang { get; set; }
        public int Nam { get; set; }
        public int DoanhThu { get; set; }
    }
    public class SPTop5
    {
        public string Ten { get; set; }
        public int Id { get; set; }
        public int SoLuong { get; set; }
    }
}
