﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataService.Migrations
{
    /// <inheritdoc />
    public partial class AddHealthDataTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HealthData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Height = table.Column<decimal>(type: "TEXT", nullable: false),
                    Weoight = table.Column<decimal>(type: "TEXT", nullable: false),
                    BloodType = table.Column<string>(type: "TEXT", nullable: false),
                    Race = table.Column<string>(type: "TEXT", nullable: false),
                    UseGlasses = table.Column<bool>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    AddDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthData", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HealthData");
        }
    }
}
