// Infrastructure/EntityConfig/UARbac/UsersConfiguration.cs
using Domain.UARbac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.UARbac
{
    public class UsersConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.ToTable("Sys_Users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.EngName)
                .HasMaxLength(200);

            builder.Property(x => x.ArbName)
                .HasMaxLength(200);

            builder.Property(x => x.ArbName4S)
                .HasMaxLength(200);

            builder.Property(x => x.Password)
                .HasMaxLength(500);

            builder.Property(x => x.Remarks)
                .HasMaxLength(500);

            builder.Property(x => x.DeviceToken)
                .HasMaxLength(500);

            builder.Property(x => x.RegDate)
                .IsRequired();

            builder.Property(x => x.RegComputerId)
                .HasMaxLength(50);

            // Indexes
            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasDatabaseName("IX_Users_Code");

            builder.HasIndex(x => x.RelEmployee)
                .HasDatabaseName("IX_Users_RelEmployee");
        }
    }
}