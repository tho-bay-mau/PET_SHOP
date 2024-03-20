using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThoBayMau_ASM.Migrations
{
    /// <inheritdoc />
    public partial class updateall : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LOAI_SP",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "NVARCHAR(100)", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAI_SP", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TAI_KHOAN",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTK = table.Column<string>(type: "Varchar(50)", nullable: false),
                    MatKhau = table.Column<string>(type: "Varchar(50)", nullable: false),
                    SDT = table.Column<string>(type: "Varchar(11)", nullable: false),
                    Email = table.Column<string>(type: "Varchar(30)", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayDangKy = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoaiTK = table.Column<bool>(type: "bit", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAI_KHOAN", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SAN_PHAM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mota = table.Column<string>(type: "ntext", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    LoaiSPId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAN_PHAM", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SAN_PHAM_LOAI_SP_LoaiSPId",
                        column: x => x.LoaiSPId,
                        principalTable: "LOAI_SP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DON_HANG",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThaiThanhToan = table.Column<bool>(type: "bit", nullable: true),
                    TrangThaiDonHang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaiKhoanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DON_HANG", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DON_HANG_TAI_KHOAN_TaiKhoanId",
                        column: x => x.TaiKhoanId,
                        principalTable: "TAI_KHOAN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LICH_SU",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThongTin_ThaoTac = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayGio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChiTiet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaiKhoanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LICH_SU", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LICH_SU_TAI_KHOAN_TaiKhoanId",
                        column: x => x.TaiKhoanId,
                        principalTable: "TAI_KHOAN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ANH",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenAnh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SanphamId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ANH", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ANH_SAN_PHAM_SanphamId",
                        column: x => x.SanphamId,
                        principalTable: "SAN_PHAM",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CHI_TIET_SP",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Gia = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    KichThuoc = table.Column<int>(type: "int", nullable: false),
                    NgaySanXuat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HanSuDung = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SanPhamId = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHI_TIET_SP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CHI_TIET_SP_SAN_PHAM_SanPhamId",
                        column: x => x.SanPhamId,
                        principalTable: "SAN_PHAM",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "THONGTIN_NHANHANG",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SDT = table.Column<int>(type: "int", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DonhangId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_THONGTIN_NHANHANG", x => x.Id);
                    table.ForeignKey(
                        name: "FK_THONGTIN_NHANHANG_DON_HANG_DonhangId",
                        column: x => x.DonhangId,
                        principalTable: "DON_HANG",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DONHANG_CHITIET",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonHangId = table.Column<int>(type: "int", nullable: false),
                    ChiTiet_SPId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DONHANG_CHITIET", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DONHANG_CHITIET_CHI_TIET_SP_ChiTiet_SPId",
                        column: x => x.ChiTiet_SPId,
                        principalTable: "CHI_TIET_SP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DONHANG_CHITIET_DON_HANG_DonHangId",
                        column: x => x.DonHangId,
                        principalTable: "DON_HANG",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ANH_SanphamId",
                table: "ANH",
                column: "SanphamId");

            migrationBuilder.CreateIndex(
                name: "IX_CHI_TIET_SP_SanPhamId",
                table: "CHI_TIET_SP",
                column: "SanPhamId");

            migrationBuilder.CreateIndex(
                name: "IX_DON_HANG_TaiKhoanId",
                table: "DON_HANG",
                column: "TaiKhoanId");

            migrationBuilder.CreateIndex(
                name: "IX_DONHANG_CHITIET_ChiTiet_SPId",
                table: "DONHANG_CHITIET",
                column: "ChiTiet_SPId");

            migrationBuilder.CreateIndex(
                name: "IX_DONHANG_CHITIET_DonHangId",
                table: "DONHANG_CHITIET",
                column: "DonHangId");

            migrationBuilder.CreateIndex(
                name: "IX_LICH_SU_TaiKhoanId",
                table: "LICH_SU",
                column: "TaiKhoanId");

            migrationBuilder.CreateIndex(
                name: "IX_SAN_PHAM_LoaiSPId",
                table: "SAN_PHAM",
                column: "LoaiSPId");

            migrationBuilder.CreateIndex(
                name: "IX_THONGTIN_NHANHANG_DonhangId",
                table: "THONGTIN_NHANHANG",
                column: "DonhangId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ANH");

            migrationBuilder.DropTable(
                name: "DONHANG_CHITIET");

            migrationBuilder.DropTable(
                name: "LICH_SU");

            migrationBuilder.DropTable(
                name: "THONGTIN_NHANHANG");

            migrationBuilder.DropTable(
                name: "CHI_TIET_SP");

            migrationBuilder.DropTable(
                name: "DON_HANG");

            migrationBuilder.DropTable(
                name: "SAN_PHAM");

            migrationBuilder.DropTable(
                name: "TAI_KHOAN");

            migrationBuilder.DropTable(
                name: "LOAI_SP");
        }
    }
}
