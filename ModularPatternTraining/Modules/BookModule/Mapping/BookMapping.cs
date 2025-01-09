using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModularPatternTraining.Modules.BookModule.Models;

namespace ModularPatternTraining.Modules.BookModule.Mapping
{
    public class BookMapping : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id);
            builder.HasIndex(x => x.Name);
            builder.Property(x => x.Name).IsUnicode();
            builder.Property(x => x.Description).HasMaxLength(256).IsRequired();

        }
    }
}
