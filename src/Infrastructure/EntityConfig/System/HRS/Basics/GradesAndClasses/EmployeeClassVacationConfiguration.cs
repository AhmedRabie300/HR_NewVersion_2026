using Domain.System.HRS.Basics.GradesAndClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.HRS.Basics.GradesAndClasses
{
    public class EmployeeClassVacationConfiguration : IEntityTypeConfiguration<EmployeeClassVacation>
    {
        public void Configure(EntityTypeBuilder<EmployeeClassVacation> builder)
        {
            builder.ToTable("hrs_EmployeesClassesVacations", tb => tb.UseSqlOutputClause(false));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.EmployeeClassId)
                .IsRequired()
                .HasColumnName("EmployeeClassID");

            builder.Property(x => x.VacationTypeId)
                .IsRequired()
                .HasColumnName("VacationTypeID");

            builder.Property(x => x.DurationDays)
                .IsRequired()
                .HasColumnName("DurationDays");

            builder.Property(x => x.RequiredWorkingMonths)
                .HasColumnName("RequiredWorkingMonths");

            builder.Property(x => x.FromMonth)
                .HasColumnType("float")
                .HasColumnName("FromMonth");

            builder.Property(x => x.ToMonth)
                .HasColumnType("float")
                .HasColumnName("ToMonth");

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

            builder.Property(x => x.TicketsRnd)
                .HasColumnName("TicketsRnd");

            builder.Property(x => x.DependantTicketRnd)
                .HasColumnName("DependantTicketRnd");

            builder.Property(x => x.MaxKeepDays)
                .HasColumnName("MaxKeepDays");

            // Relationships
            builder.HasOne(x => x.EmployeeClass)
                .WithMany(x => x.Vacations)
                .HasForeignKey(x => x.EmployeeClassId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.VacationType)
                .WithMany()
                .HasForeignKey(x => x.VacationTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.EmployeeClassId)
                .HasDatabaseName("IX_EmployeeClassVacation_EmployeeClassId");

            builder.HasIndex(x => x.VacationTypeId)
                .HasDatabaseName("IX_EmployeeClassVacation_VacationTypeId");
        }
    }
}