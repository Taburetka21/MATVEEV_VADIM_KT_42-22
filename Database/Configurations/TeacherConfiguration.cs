using MatveevVadimKt_42_22.Database.Helpers;
using MatveevVadimKt_42_22.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MatveevVadimKt_42_22.Database.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        private const string TableName = "Teachers";
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            //Первичный ключ
            builder
                .HasKey(p => p.Id)
                .HasName($"pl_{TableName}_teacher_id");

            //Автоинкрементация
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("Teacher_Id")
                .HasComment("Id преподавателя");

            // Колонка имени
            builder.Property(p => p.FirstName)
                .IsRequired()
                .HasColumnName("First_Name")
                .HasColumnType(ColumnType.String).HasMaxLength(20)
                .HasComment("Имя преподавателя");

            // Колонка отчества
            builder.Property(p => p.LastName)
                .IsRequired()
                .HasColumnName("Last_Name")
                .HasColumnType(ColumnType.String).HasMaxLength(20)
                .HasComment("Фамилия преподавателя");

            // Колонка с уч. степенью
            builder.Property(p => p.DegreeId)
                .IsRequired()
                .HasColumnName("Degree_Id")
                .HasColumnType(ColumnType.Int)
                .HasComment("Id ученой степени");

            // Настройка связи (при удалении летит в null)
            builder.HasOne(p => p.Degree)
                .WithMany()
                .HasForeignKey(p => p.DegreeId)
                .HasConstraintName("fk_f_degree_id")
                .OnDelete(DeleteBehavior.Cascade); //Пересмотреть

            builder.ToTable(TableName)
                .HasIndex(p => p.DegreeId, $"idx_{TableName}_fk_f_degree_id");

            builder.Navigation(p => p.Degree)
                .AutoInclude();

            // Колонка с должностью
            builder.Property(p => p.PositionId)
                .IsRequired()
                .HasColumnName("Position_Id")
                .HasColumnType(ColumnType.Int)
                .HasComment("Id занимаемой должности");

            // Настройка связи (при удалении препод тоже удаляется)
            builder.HasOne(p => p.Position)
                .WithMany()
                .HasForeignKey(p => p.PositionId)
                .HasConstraintName("fk_f_position_id")
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable(TableName)
                .HasIndex(p => p.DegreeId, $"idx_{TableName}_fk_f_position_id");

            builder.Navigation(p => p.Position)
                .AutoInclude();

            //  Колонка с кафедрой
            builder.Property(p => p.DepartmentId)
                .IsRequired()
                .HasColumnName("Department_Id")
                .HasColumnType(ColumnType.Int)
                .HasComment("Id кафедры");

            // Настройка связи (при удалении летит в null)
            builder.HasOne(p => p.Department)
                .WithMany()
                .HasForeignKey(p => p.DepartmentId)
                .HasConstraintName("fk_f_department_id")
                .OnDelete(DeleteBehavior.Cascade); //Пересмотреть

            builder.ToTable(TableName)
                .HasIndex(p => p.DepartmentId, $"idx_{TableName}_fk_f_department_id");
        }
    }
}
