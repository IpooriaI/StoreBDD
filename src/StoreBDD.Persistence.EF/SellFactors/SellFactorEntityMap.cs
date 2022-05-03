using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreBDD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreBDD.Persistence.EF.SellFactors
{
    public class SellFactorEntityMap : IEntityTypeConfiguration<SellFactor>
    {
        public void Configure(EntityTypeBuilder<SellFactor> builder)
        {
            builder.ToTable("SellFactors");

            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(_ => _.Product);

        }
    }
}
