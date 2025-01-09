using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModularPatternTraining.Modules.LibraryModule.Models;

namespace ModularPatternTraining.Modules.LibraryModule.Mapping
{
    public class LibraryMapping : IEntityTypeConfiguration<Library>
    {
        public void Configure(EntityTypeBuilder<Library> builder)
        {
            builder.ToTable("Libraries");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id);
            builder.HasIndex(x => x.Name);
            builder.Property(x => x.Name).IsUnicode();
            builder.Property(x => x.description).HasMaxLength(256).IsRequired();

            builder.HasMany(x => x.Books).WithOne(x => x.Library).HasForeignKey(x => x.LibraryId);
        }
    }
}
