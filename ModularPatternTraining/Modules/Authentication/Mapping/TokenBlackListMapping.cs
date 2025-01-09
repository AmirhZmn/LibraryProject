using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModularPatternTraining.Modules.Authentication.Models;

namespace ModularPatternTraining.Modules.Authentication.Mapping
{
    public class TokenBlackListMapping : IEntityTypeConfiguration<TokenBlackListModel>
    {
        public void Configure(EntityTypeBuilder<TokenBlackListModel> builder)
        {
            builder.ToTable("TokenBlackList");
            builder.HasKey(e => e.Id);
            builder.Property(x => x.jti).IsRequired().HasMaxLength(255);
        }
    }
}
