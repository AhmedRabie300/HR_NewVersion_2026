// Infrastructure/EntityConfig/UARbac/UserGroupConfiguration.cs
using Domain.UARbac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.UARbac
{
    public class UserGroupConfiguration : IEntityTypeConfiguration<UserGroup>
    {
        public void Configure(EntityTypeBuilder<UserGroup> builder)
        {
            builder.ToTable("Sys_GroupsUsers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.GroupId)
                .IsRequired();

     

            builder.Property(x => x.RegDate)
                .IsRequired();

            // Relationships
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

             builder.HasOne(x => x.Group)
                .WithMany(x => x.GroupUsers)   
                .HasForeignKey(x => x.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => new { x.UserId, x.GroupId })
                .IsUnique()
                .HasDatabaseName("IX_UserGroup_UserId_GroupId");

            builder.HasIndex(x => x.UserId)
                .HasDatabaseName("IX_UserGroup_UserId");

            builder.HasIndex(x => x.GroupId)
                .HasDatabaseName("IX_UserGroup_GroupId");

        
        }
    }
}