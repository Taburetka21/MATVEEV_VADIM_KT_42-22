using MatveevVadimKt_42_22.Database.Helpers;
using MatveevVadimKt_42_22.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MatveevVadimKt_42_22.Database.Configurations
{
    public class DegreeConfiguration : IEntityTypeConfiguration<Degree>
    {
        private const string TableName = "Degree";
        public void Configure(EntityTypeBuilder<Degree> builder)
        {
            builder.HasKey(p => p.Id)
           .HasName($"pl_{TableName}_degree_id");

            //Автоинкрементация
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("degree_Id")
                .HasComment("Id уч. степени");

            builder.Property(p => p.Name)
                 .IsRequired()
                 .HasColumnName("Name")
                 .HasColumnType(ColumnType.String).HasMaxLength(50)
                 .HasComment("Название уч. степени");

            builder.ToTable(TableName);
        }
    }
}
