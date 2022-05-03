using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreBDD.Entities;

namespace StoreBDD.Persistence.EF.BuyFactors
{
    public class BuyFactorEntityMap : IEntityTypeConfiguration<BuyFactor>
    {
        public void Configure(EntityTypeBuilder<BuyFactor> builder)
        {
            builder.ToTable("BuyFactors");

            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(_ => _.Product);
        }
    }
}
