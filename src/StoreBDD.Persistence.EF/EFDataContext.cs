using Microsoft.EntityFrameworkCore;
using StoreBDD.Entities;
using StoreBDD.Persistence.EF.Categories;

namespace StoreBDD.Persistence.EF
{
    public class EFDataContext : DbContext
    {

        public EFDataContext(string connectionString) :
            this(new DbContextOptionsBuilder().UseSqlServer(connectionString).Options)
        { }

        public EFDataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly
                (typeof(CategoryEntityMap).Assembly);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SellFactor> SellFactors { get; set; }
        public DbSet<BuyFactor> BuyFactors { get; set; }

    }
}
