using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModularPatternTraining.Modules.UserManagement.Model;

namespace ModularPatternTraining.Modules.UserManagement.Mapping
{
    public class ApplicationUserMapping : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(p => p.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(p => p.LastName).HasMaxLength(100).IsRequired();
            builder.Property(p => p.Address).HasMaxLength(500).IsRequired(false);
            builder.Property(p => p.NationalCode).HasMaxLength(10).IsRequired();
        }
    }
}
