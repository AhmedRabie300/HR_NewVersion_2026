using Domain.System.HRS.Basics.GradesAndClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.HRS.Basics.GradesAndClasses
{
    public class EmployeeClassDelayConfiguration : IEntityTypeConfiguration<EmployeeClassDelay>
    {
        public void Configure(EntityTypeBuilder<EmployeeClassDelay> builder)
        {
            builder.ToTable("hrs_EmployeesClassesDelay", tb => tb.UseSqlOutputClause(false));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ClassId)
                .HasColumnName("ClassID");

            builder.Property(x => x.FromMin)
                .HasColumnName("FromMin");

            builder.Property(x => x.ToMin)
                .HasColumnName("ToMin");

            builder.Property(x => x.PunishPCT)
                .HasColumnName("PunishPCT");

            builder.Property(x => x.CompanyId)
                .HasColumnName("CompanyID");

            builder.Property(x => x.Remarks)
                .HasMaxLength(2048)
                .HasColumnName("Remarks");

            builder.Property(x => x.RegUserId)
                .HasColumnName("RegUserID");

            builder.Property(x => x.RegComputerId)
                .HasColumnName("RegComputerID");

            builder.Property(x => x.RegDate)
                .IsRequired()
                .HasColumnName("RegDate")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.CancelDate)
                .HasColumnName("CancelDate");

            // Relationships
            builder.HasOne(x => x.EmployeeClass)
                .WithMany(x => x.Delays)
                .HasForeignKey(x => x.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.ClassId)
                .HasDatabaseName("IX_EmployeeClassDelay_ClassId");
        }
    }
}