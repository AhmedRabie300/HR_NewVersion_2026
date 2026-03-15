using Domain.System.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.MasterData
{
    public class NationalityConfiguration : IEntityTypeConfiguration<Nationality>
    {
        public void Configure(EntityTypeBuilder<Nationality> builder)
        {
            // 1. اسم الجدول
            builder.ToTable("sys_Nationalities");

            // 2. المفتاح الأساسي
            builder.HasKey(x => x.Id);

            // 3. تهيئة الخصائص
            builder.Property(x => x.Code)
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnName("Code");

            builder.Property(x => x.EngName)
                .HasMaxLength(100)
                .HasColumnName("EngName");

            builder.Property(x => x.ArbName)
                .HasMaxLength(100)
                .HasColumnName("ArbName");

            builder.Property(x => x.ArbName4S)
                .HasMaxLength(100)
                .HasColumnName("ArbName4S");

            builder.Property(x => x.IsMainNationality)
                .HasColumnName("IsMainNationality");

            builder.Property(x => x.TravelRoute)
                .HasColumnName("TravelRoute");

            builder.Property(x => x.TravelClass)
                .HasColumnName("TravelClass");

            builder.Property(x => x.Remarks)
                .HasMaxLength(2048)
                .HasColumnName("Remarks");

            builder.Property(x => x.RegUserId)
                .HasColumnName("RegUserID");

            builder.Property(x => x.RegComputerId)
                .HasMaxLength(50)
                .HasColumnName("RegComputerID");

            builder.Property(x => x.RegDate)
                .IsRequired()
                .HasColumnName("RegDate")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.CancelDate)
                .HasColumnName("CancelDate");

            builder.Property(x => x.TicketAmount)
                .HasColumnName("TicketAmount");

            // 4. الفهارس (Indexes)
            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasDatabaseName("IX_Nationalities_Code");

            builder.HasIndex(x => x.EngName)
                .HasDatabaseName("IX_Nationalities_EngName");

            builder.HasIndex(x => x.ArbName)
                .HasDatabaseName("IX_Nationalities_ArbName");

            builder.HasIndex(x => x.TravelRoute)
                .HasDatabaseName("IX_Nationalities_TravelRoute");

           
        }
    }
}