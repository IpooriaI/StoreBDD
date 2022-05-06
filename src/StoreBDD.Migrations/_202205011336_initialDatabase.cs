using FluentMigrator;

namespace StoreBDD.Migrations
{
    [Migration(202205011336)]
    public class _202205011336_initialDatabase : Migration
    {

        public override void Up()
        {
            Create.Table("Categories")
                .WithColumn("Id").AsInt32().Identity().NotNullable().PrimaryKey()
                .WithColumn("Title").AsString(30).NotNullable();
            Create.Table("Products")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey()
                .WithColumn("Name").AsString(50).NotNullable()
                .WithColumn("Price").AsInt32().NotNullable()
                .WithColumn("Count").AsInt32().NotNullable()
                .WithColumn("MinimumCount").AsInt32().NotNullable()
                .WithColumn("CategoryId").AsInt32().NotNullable()
                .ForeignKey("FK_Products_Categories", "Categories", "Id");
            Create.Table("BuyFactors")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Count").AsInt32().NotNullable()
                .WithColumn("DateBought").AsDate().NotNullable()
                .WithColumn("ProductId").AsInt32().NotNullable()
                .ForeignKey("FK_BuyFactors_Products", "Products", "Id");
            Create.Table("SellFactors")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Count").AsInt32().NotNullable()
                .WithColumn("DateSold").AsDate().NotNullable()
                .WithColumn("ProductId").AsInt32().NotNullable()
                .ForeignKey("FK_SellFactors_Products", "Products", "Id");
        }
        public override void Down()
        {
            Delete.Table("Categories");
            Delete.Table("Products");
            Delete.Table("BuyFactors");
            Delete.Table("SellFactors");
        }
    }
}
