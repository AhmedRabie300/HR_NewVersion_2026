using Domain.System.HRS.Basics.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.System.HRS.Basics.Contracts; 
    namespace Infrastructure.EntityConfig.System.HRS.Basics.Contracts
{
    public class ContractConfiguration : IEntityTypeConfiguration<Domain.System.HRS.Basics.Contracts.Contract>
    {
        public void Configure(EntityTypeBuilder<Domain.System.HRS.Basics.Contracts.Contract> builder)
        {
            builder.ToTable("hrs_Contracts", tb => tb.UseSqlOutputClause(false));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Number)
                .IsRequired()
                .HasColumnName("Number");

            builder.Property(x => x.ContractTypeId)
                .IsRequired()
                .HasColumnName("ContractTypeID");

            builder.Property(x => x.EmployeeClassId)
                .IsRequired()
                .HasColumnName("EmployeeClassID");

            builder.Property(x => x.EmployeeId)
                .IsRequired()
                .HasColumnName("EmployeeID");

            builder.Property(x => x.StartDate)
                .IsRequired()
                .HasColumnName("StartDate");

            builder.Property(x => x.EndDate)
                .HasColumnName("EndDate");

            builder.Property(x => x.ProfessionId)
                .HasColumnName("ProfessionID");

            builder.Property(x => x.PositionId)
                .HasColumnName("PositionID");

            builder.Property(x => x.GradeStepId)
                .HasColumnName("GradeStepID");

            builder.Property(x => x.CurrencyId)
                .HasColumnName("CurrencyID");

            builder.Property(x => x.Remarks)
                .HasMaxLength(2048)
                .HasColumnName("Remarks");

            builder.Property(x => x.RegUserId)
                .HasColumnName("RegUserID");

            builder.Property(x => x.RegComputerId)
                .HasColumnName("RegComputerID");

            builder.Property(x => x.RegDate)
                .HasColumnName("RegDate")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.CancelDate)
                .HasColumnName("CancelDate");

            builder.Property(x => x.ContractPeriod)
                .HasColumnName("ContractPeriod");

            builder.Property(x => x.UpdatedUserId)
                .HasColumnName("UpdatedUserID");

            builder.Property(x => x.UpdateDate)
                .HasColumnName("UpdateDate");

            builder.Property(x => x.CompanyId)
                .IsRequired()
                .HasColumnName("CompanyID");

            // Relationships
            builder.HasOne(x => x.Company)
                .WithMany()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ContractType)
                .WithMany()
                .HasForeignKey(x => x.ContractTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.EmployeeClass)
                .WithMany()
                .HasForeignKey(x => x.EmployeeClassId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Profession)
                .WithMany()
                .HasForeignKey(x => x.ProfessionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Position)
                .WithMany()
                .HasForeignKey(x => x.PositionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.GradeStep)
                .WithMany()
                .HasForeignKey(x => x.GradeStepId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Currency)
                .WithMany()
                .HasForeignKey(x => x.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.Number)
                .HasDatabaseName("IX_Contracts_Number");

            builder.HasIndex(x => x.EmployeeId)
                .HasDatabaseName("IX_Contracts_EmployeeId");

            builder.HasIndex(x => x.ContractTypeId)
                .HasDatabaseName("IX_Contracts_ContractTypeId");

            builder.HasIndex(x => x.CompanyId)
                .HasDatabaseName("IX_Contracts_CompanyId");
        }
    }
}