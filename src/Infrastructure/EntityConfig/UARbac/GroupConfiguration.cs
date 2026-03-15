// Infrastructure/EntityConfig/UARbac/GroupConfiguration.cs
using Domain.UARbac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.UARbac
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable("Sys_Groups");  

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.EngName)
                .HasMaxLength(200);

            builder.Property(x => x.ArbName)
                .HasMaxLength(200);

            builder.Property(x => x.RegDate)
                .IsRequired();

            builder.Property(x => x.RegUserId);

            builder.Property(x => x.CancelDate);

            // Indexes
            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasDatabaseName("IX_Groups_Code");

         }
    }
}