using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModularPatternTraining.Modules.Authentication.Models;

namespace ModularPatternTraining.Modules.Authentication.Mapping
{
    public class RefreshTokenMapping : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Token).HasMaxLength(500);

            builder.HasOne(x => x.User).WithMany(x => x.Tokens).HasForeignKey(x => x.UserId);

        }
    }
}
