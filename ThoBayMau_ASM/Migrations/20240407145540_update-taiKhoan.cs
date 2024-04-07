using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThoBayMau_ASM.Migrations
{
    /// <inheritdoc />
    public partial class updatetaiKhoan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MatKhau",
                table: "TAI_KHOAN",
                type: "Varchar(60)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "Varchar(50)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MatKhau",
                table: "TAI_KHOAN",
                type: "Varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "Varchar(60)");
        }
    }
}
