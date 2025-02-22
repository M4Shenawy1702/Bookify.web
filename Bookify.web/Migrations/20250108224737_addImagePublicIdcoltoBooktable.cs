﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.web.Migrations
{
    /// <inheritdoc />
    public partial class addImagePublicIdcoltoBooktable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePublicId",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePublicId",
                table: "Books");
        }
    }
}
