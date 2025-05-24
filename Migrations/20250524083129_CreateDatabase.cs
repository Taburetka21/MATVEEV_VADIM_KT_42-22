using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MatveevVadimKt_42_22.Migrations
{
    public partial class CreateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Degree",
                columns: table => new
                {
                    degree_Id = table.Column<int>(type: "integer", nullable: false, comment: "Id уч. степени")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar", maxLength: 50, nullable: false, comment: "Название уч. степени")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pl_Degree_degree_id", x => x.degree_Id);
                });

            migrationBuilder.CreateTable(
                name: "Disciplines",
                columns: table => new
                {
                    Discipline_Id = table.Column<int>(type: "integer", nullable: false, comment: "Id дисциплины")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Discipline_Name = table.Column<string>(type: "varchar", maxLength: 100, nullable: false, comment: "Название дисциплины")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pl_Disciplines_discipline_id", x => x.Discipline_Id);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Position_Id = table.Column<int>(type: "integer", nullable: false, comment: "Id должности")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Position_Name = table.Column<string>(type: "varchar", maxLength: 100, nullable: false, comment: "Название должности")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pl_Positions_position_id", x => x.Position_Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Department_Id = table.Column<int>(type: "integer", nullable: false, comment: "Id факультета")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar", maxLength: 20, nullable: false, comment: "Название факультета"),
                    Founded_Date = table.Column<DateTime>(type: "timestamp", nullable: false, comment: "Дата основания факультета"),
                    Head_Id = table.Column<int>(type: "integer", nullable: true, comment: "Id зав. кафедры")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pl_Departments_department_id", x => x.Department_Id);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Teacher_Id = table.Column<int>(type: "integer", nullable: false, comment: "Id преподавателя")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    First_Name = table.Column<string>(type: "varchar", maxLength: 20, nullable: false, comment: "Имя преподавателя"),
                    Last_Name = table.Column<string>(type: "varchar", maxLength: 20, nullable: false, comment: "Фамилия преподавателя"),
                    Degree_Id = table.Column<int>(type: "int4", nullable: false, comment: "Id ученой степени"),
                    Position_Id = table.Column<int>(type: "int4", nullable: false, comment: "Id занимаемой должности"),
                    Department_Id = table.Column<int>(type: "int4", nullable: false, comment: "Id кафедры")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pl_Teachers_teacher_id", x => x.Teacher_Id);
                    table.ForeignKey(
                        name: "fk_f_degree_id",
                        column: x => x.Degree_Id,
                        principalTable: "Degree",
                        principalColumn: "degree_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_f_department_id",
                        column: x => x.Department_Id,
                        principalTable: "Departments",
                        principalColumn: "Department_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_f_position_id",
                        column: x => x.Position_Id,
                        principalTable: "Positions",
                        principalColumn: "Position_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Loads",
                columns: table => new
                {
                    Load_Id = table.Column<int>(type: "integer", nullable: false, comment: "Id нагрузки")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Teacher_Id = table.Column<int>(type: "int4", nullable: false, comment: "Id преподавателя"),
                    Discipline_Id = table.Column<int>(type: "int4", nullable: false, comment: "Id дисциплины"),
                    Hours = table.Column<int>(type: "int4", nullable: false, comment: "Количество часов нагрузки")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pl_Loads_load_id", x => x.Load_Id);
                    table.ForeignKey(
                        name: "fk_f_discipline_id",
                        column: x => x.Discipline_Id,
                        principalTable: "Disciplines",
                        principalColumn: "Discipline_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_f_teacher_id",
                        column: x => x.Teacher_Id,
                        principalTable: "Teachers",
                        principalColumn: "Teacher_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_Departments_fk_f_head_id",
                table: "Departments",
                column: "Head_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Head_Id",
                table: "Departments",
                column: "Head_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_Disciplines_name",
                table: "Disciplines",
                column: "Discipline_Name");

            migrationBuilder.CreateIndex(
                name: "idx_Loads_fk_f_discipline_id",
                table: "Loads",
                column: "Discipline_Id");

            migrationBuilder.CreateIndex(
                name: "idx_Loads_fk_f_teacher_id",
                table: "Loads",
                column: "Teacher_Id");

            migrationBuilder.CreateIndex(
                name: "idx_Teachers_fk_f_degree_id",
                table: "Teachers",
                column: "Degree_Id");

            migrationBuilder.CreateIndex(
                name: "idx_Teachers_fk_f_department_id",
                table: "Teachers",
                column: "Department_Id");

            migrationBuilder.CreateIndex(
                name: "idx_Teachers_fk_f_position_id",
                table: "Teachers",
                column: "Degree_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_Position_Id",
                table: "Teachers",
                column: "Position_Id");

            migrationBuilder.AddForeignKey(
                name: "fk_f_head_id",
                table: "Departments",
                column: "Head_Id",
                principalTable: "Teachers",
                principalColumn: "Teacher_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_f_head_id",
                table: "Departments");

            migrationBuilder.DropTable(
                name: "Loads");

            migrationBuilder.DropTable(
                name: "Disciplines");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Degree");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Positions");
        }
    }
}
