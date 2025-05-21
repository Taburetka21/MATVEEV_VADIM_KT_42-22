using MatveevVadimKt_42_22.Database.Helpers;
using MatveevVadimKt_42_22.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MatveevVadimKt_42_22.Database.Configurations
{
   
        public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
        {
            private const string TableName = "Departments";
            public void Configure(EntityTypeBuilder<Department> builder)
            {
                builder.HasKey(p => p.Id)
                .HasName($"pl_{TableName}_department_id");

                //Автоинкрементация
                builder.Property(p => p.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("Department_Id")
                    .HasComment("Id факультета");

                builder.Property(p => p.Name)
                     .IsRequired()
                     .HasColumnName("Name")
                     .HasColumnType(ColumnType.String).HasMaxLength(20)
                     .HasComment("Название факультета");

                builder.Property(p => p.FoundedDate)
                     .IsRequired()
                     .HasColumnName("Founded_Date")
                     .HasColumnType(ColumnType.Date)
                     .HasComment("Дата основания факультета");

                // Настройка связи
                builder.Property(p => p.HeadId)
                  .HasColumnName("Head_Id")
                  .HasComment("Id зав. кафедры");


                builder.HasOne(p => p.Head)
                    .WithOne()
                    .HasForeignKey<Department>(p => p.HeadId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("fk_f_head_id");


                builder.ToTable(TableName)
                    .HasIndex(p => p.HeadId, $"idx_{TableName}_fk_f_head_id");
            }
        }
    
}
