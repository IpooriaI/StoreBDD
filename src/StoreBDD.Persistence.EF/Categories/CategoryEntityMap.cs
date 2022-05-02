using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreBDD.Entities;

namespace StoreBDD.Persistence.EF.Categories
{
    public class CategoryEntityMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id)
                .ValueGeneratedOnAdd();

            builder.HasMany(_ => _.Products)
                .WithOne(_ => _.Category)
                .HasForeignKey(_ => _.CategoryId);
        }
    }
}
