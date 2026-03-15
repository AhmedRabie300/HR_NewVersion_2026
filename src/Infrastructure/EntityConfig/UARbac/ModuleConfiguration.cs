using Domain.UARbac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.UARbac
{
    public class ModuleConfiguration : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
             builder.ToTable("sys_Modules");

             builder.HasKey(x => x.Id);

             builder.Property(x => x.Code)
                .HasMaxLength(50)
                .IsRequired()  
                .HasColumnName("Code"); 

            builder.Property(x => x.Prefix)
                .HasMaxLength(30)
                .HasColumnName("Prefix");

            builder.Property(x => x.EngName)
                .HasMaxLength(100)
                .HasColumnName("EngName");

            builder.Property(x => x.ArbName)
                .HasMaxLength(100)
                .HasColumnName("ArbName");

            builder.Property(x => x.ArbName4S)
                .HasMaxLength(100)
                .HasColumnName("ArbName4S");

            builder.Property(x => x.FormId)
                .HasColumnName("FormID");  

            builder.Property(x => x.IsRegistered)
                .HasColumnName("IsRegistered");

            builder.Property(x => x.FiscalYearDependant)
                .HasColumnName("FiscalYearDependant");

            builder.Property(x => x.Rank)
                .HasColumnName("Rank");

            builder.Property(x => x.Remarks)
                .HasMaxLength(2048)
                .HasColumnName("Remarks");

             builder.Property(x => x.IsAR)
                .HasColumnName("ISAR");

            builder.Property(x => x.IsAP)
                .HasColumnName("ISAP");

            builder.Property(x => x.IsGL)
                .HasColumnName("IsGL");

            builder.Property(x => x.IsFA)
                .HasColumnName("ISFA");

            builder.Property(x => x.IsINV)
                .HasColumnName("IsINV");

            builder.Property(x => x.IsHR)
                .HasColumnName("IsHR");

            builder.Property(x => x.IsMANF)
                .HasColumnName("ISMANF");

            builder.Property(x => x.IsSYS)
                .HasColumnName("IsSYS");

             builder.Property(x => x.RegUserId)
                .HasColumnName("RegUserID");

            builder.Property(x => x.RegComputerId)
                .HasMaxLength(50)
                .HasColumnName("RegComputerID");

            builder.Property(x => x.RegDate)
                .HasColumnName("RegDate")
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");  

            builder.Property(x => x.CancelDate)
                .HasColumnName("CancelDate");

             builder.HasIndex(x => x.Code)
                .IsUnique()  
                .HasDatabaseName("IX_Modules_Code");

            builder.HasIndex(x => x.FormId)
                .HasDatabaseName("IX_Modules_FormId");

            builder.HasIndex(x => x.Rank)
                .HasDatabaseName("IX_Modules_Rank");
 
        }
    }
}