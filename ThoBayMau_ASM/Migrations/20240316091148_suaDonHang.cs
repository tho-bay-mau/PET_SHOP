using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThoBayMau_ASM.Migrations
{
    /// <inheritdoc />
    public partial class suaDonHang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DONHANG_CHITIET_SAN_PHAM_SanPhamId",
                table: "DONHANG_CHITIET");

            migrationBuilder.RenameColumn(
                name: "SanPhamId",
                table: "DONHANG_CHITIET",
                newName: "ChiTiet_SPId");

            migrationBuilder.RenameIndex(
                name: "IX_DONHANG_CHITIET_SanPhamId",
                table: "DONHANG_CHITIET",
                newName: "IX_DONHANG_CHITIET_ChiTiet_SPId");

            migrationBuilder.AddForeignKey(
                name: "FK_DONHANG_CHITIET_CHI_TIET_SP_ChiTiet_SPId",
                table: "DONHANG_CHITIET",
                column: "ChiTiet_SPId",
                principalTable: "CHI_TIET_SP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DONHANG_CHITIET_CHI_TIET_SP_ChiTiet_SPId",
                table: "DONHANG_CHITIET");

            migrationBuilder.RenameColumn(
                name: "ChiTiet_SPId",
                table: "DONHANG_CHITIET",
                newName: "SanPhamId");

            migrationBuilder.RenameIndex(
                name: "IX_DONHANG_CHITIET_ChiTiet_SPId",
                table: "DONHANG_CHITIET",
                newName: "IX_DONHANG_CHITIET_SanPhamId");

            migrationBuilder.AddForeignKey(
                name: "FK_DONHANG_CHITIET_SAN_PHAM_SanPhamId",
                table: "DONHANG_CHITIET",
                column: "SanPhamId",
                principalTable: "SAN_PHAM",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
