using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreBDD.Entities;

namespace StoreBDD.Persistence.EF.Products
{
    public class ProductEntityMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(_ => _.Category)
                .WithMany(_ => _.Products)
                .HasForeignKey(_ => _.CategoryId);
        }
    }
}
