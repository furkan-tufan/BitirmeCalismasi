using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BitirmeProjesi.Data.Migrations
{
    public partial class removeApplicationProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Application",
                table: "Course");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "EmployeeCourses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "EmployeeCourses");

            migrationBuilder.AddColumn<string>(
                name: "Application",
                table: "Course",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
