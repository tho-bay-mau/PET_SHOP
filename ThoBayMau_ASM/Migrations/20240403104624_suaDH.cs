using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThoBayMau_ASM.Migrations
{
    /// <inheritdoc />
    public partial class suaDH : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiGianHuy",
                table: "DON_HANG",
                type: "datetime2",
                nullable: true);

           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "ThoiGianHuy",
                table: "DON_HANG");
        }
    }
}
