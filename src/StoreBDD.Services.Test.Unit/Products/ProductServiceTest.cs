using FluentAssertions;
using StoreBDD.Infrastructure.Test;
using StoreBDD.Persistence.EF;
using StoreBDD.Persistence.EF.BuyFactors;
using StoreBDD.Persistence.EF.Products;
using StoreBDD.Persistence.EF.SellFactors;
using StoreBDD.Services.Products;
using StoreBDD.Services.Products.Contracts;
using StoreBDD.Services.Products.Exceptions;
using StoreBDD.Test.Tools.Categories;
using StoreBDD.Test.Tools.Products;
using System;
using System.Linq;
using Xunit;


namespace StoreBDD.Services.Test.Unit.Products
{
    public class ProductServiceTest
    {
        private readonly EFDataContext _dataContext;
        private readonly ProductService _sut;
        public ProductServiceTest()
        {
            _dataContext =
                new EFInMemoryDatabase().CreateDataContext<EFDataContext>();
            var _unitOfWork = new EFUnitOfWork(_dataContext);
            var _repository = new EFProductRepository(_dataContext);
            var _sellRepository = new EFSellFactorRepository(_dataContext);
            var _buyRepository = new EFBuyFactorRepository(_dataContext);
            _sut = new ProductAppService(_repository,
                _unitOfWork, _sellRepository, _buyRepository);
        }

        [Fact]
        public void Add_adds_Product_properly()
        {
            var category = CategoryFactory.GenerateCategory("DummyTitle");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var dto = ProductFactory
                .GenerateAddProductDto("TestName", category.Id);

            _sut.Add(dto);

            _dataContext.Products.Should().HaveCount(1);
            _dataContext.Products.Should().Contain(_ => _.Price == dto.Price);
            _dataContext.Products.Should().Contain(_ => _.Name == dto.Name);
            _dataContext.Products.Should()
                .Contain(_ => _.CategoryId == dto.CategoryId);
            _dataContext.Products.Should()
                .Contain(_ => _.MinimumCount == dto.MinimumCount);
        }

        [Fact]
        public void Add_throws_exception_DuplicateProductNameInSameCategoryException_if_product_with_same_name_already_exists_in_this_category()
        {
            var category = CategoryFactory.GenerateCategory("DummyTitle");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var product = ProductFactory.GenerateProduct("Test", category.Id,5);
            _dataContext.Manipulate(_ => _.Products.Add(product));
            var dto = ProductFactory
                .GenerateAddProductDto(product.Name, category.Id);

            Action expected = () => _sut.Add(dto);

            expected.Should()
                .ThrowExactly<DuplicateProductNameInSameCategoryException>();
        }

        [Fact]
        public void Add_throws_DuplicateProductIdException_if_productid_already_exists()
        {
            var category = CategoryFactory.GenerateCategory("DummyTitle");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var product = ProductFactory.GenerateProduct("Test", category.Id, 2);
            _dataContext.Manipulate(_ => _.Products.AddRange(product));
            var dto = ProductFactory
                .GenerateAddProductDto("DummyName",category.Id,product.Id);

            Action expected = () => _sut.Add(dto);

            expected.Should().ThrowExactly<DuplicateProductIdException>();
        }

        [Fact]
        public void Add_throws_CategoryNotFoundException_if_category_dosnt_exist()
        {
            var fakeCategoryId = 40;
            var dto = ProductFactory
                .GenerateAddProductDto("DummyName", fakeCategoryId);

            Action expected = () => _sut.Add(dto);

            expected.Should().ThrowExactly<CategoryNotFoundException>();
        }

        [Fact]
        public void Update_updates_the_Product_properly()
        {
            var product = ProductFactory.GenerateProductWithCategory("Test",4);
            _dataContext.Manipulate(_ => _.Products.Add(product));
            var dto = ProductFactory
                .GenerateUpdateProductDto("UpdatedName", product.CategoryId);

            _sut.Update(product.Id, dto);

            _dataContext.Products.Should().HaveCount(1);
            _dataContext.Products.Should().Contain(_ => _.Price == dto.Price);
            _dataContext.Products.Should().Contain(_ => _.Name == dto.Name);
            _dataContext.Products.Should()
                .Contain(_ => _.CategoryId == dto.CategoryId);
            _dataContext.Products.Should()
                .Contain(_ => _.MinimumCount == dto.MinimumCount);
        }

        [Fact]
        public void Update_throws_exception_DuplicateProductNameInSameCategoryException_if_product_with_same_name_exists()
        {
            var category = CategoryFactory.GenerateCategory("DummyTitle");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var product = ProductFactory.GenerateProduct("Test", category.Id,2);
            var product2 = ProductFactory.GenerateProduct("Test2", category.Id,3);
            _dataContext.Manipulate(_ => _.Products.AddRange(product, product2));
            var dto = ProductFactory
                .GenerateUpdateProductDto(product2.Name, category.Id);

            Action expected = () => _sut.Update(product.Id, dto);

            expected.Should()
                .ThrowExactly<DuplicateProductNameInSameCategoryException>();
        }

        [Fact]
        public void Update_throws_exception_ProductNotFoundException_if_product_dosnt_exist()
        {
            var category = CategoryFactory.GenerateCategory("DummyTitle");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var fakeProductId = 20;
            var dto = ProductFactory
                .GenerateUpdateProductDto("Dummy", category.Id);

            Action expected = () => _sut.Update(fakeProductId, dto);

            expected.Should().ThrowExactly<ProductNotFoundException>();
        }

        [Fact]
        public void Update_throws_CategoryNotFoundException_if_category_dosnt_exist()
        {
            var fakeCategoryId = 40;
            var product = ProductFactory.GenerateProductWithCategory("test", 2);
            _dataContext.Manipulate(_ => _.Products.Add(product));
            var dto = ProductFactory
                .GenerateUpdateProductDto("DummyName", fakeCategoryId);

            Action expected = () => _sut.Update(product.Id,dto);

            expected.Should().ThrowExactly<CategoryNotFoundException>();
        }

        [Fact]
        public void Delete_deletes_the_Product_properly()
        {
            var product = ProductFactory.GenerateProductWithCategory("Dummy",2);
            _dataContext.Manipulate(_ => _.Products.Add(product));

            _sut.Delete(product.Id);

            _dataContext.Products.Count().Should().Be(0);
            _dataContext.Products
                .Should().NotContain(_ => _.Name == product.Name);
        }

        [Fact]
        public void Delete_throws_exception_ProductNotFoundException_if_product_dosnt_exist()
        {
            var fakeProductId = 23;

            Action expected = () => _sut.Delete(fakeProductId);

            expected.Should().ThrowExactly<ProductNotFoundException>();
        }

        [Fact]
        public void Get_returns_a_product_properly()
        {
            var product = ProductFactory.GenerateProductWithCategory("Test",3);
            _dataContext.Manipulate(_ => _.Products.Add(product));

            var expected = _sut.Get(product.Id);

            expected.Name.Should().Be(product.Name);
            expected.Count.Should().Be(product.Count);
            expected.CategoryId.Should().Be(product.CategoryId);
            expected.Price.Should().Be(product.Price);
        }

        [Fact]
        public void Sell_sells_the_Product_properly()
        {
            var product = ProductFactory.GenerateProductWithCategory("Test",3);
            var count = product.Count;
            _dataContext.Manipulate(_ => _.Products.Add(product));
            var dto = ProductFactory.GenerateSellProductDto(2);

            _sut.Sell(product.Id, dto);

            _dataContext.SellFactors.Should().HaveCount(1);
            _dataContext.Products.Should()
                .Contain(_ => _.Count == count - dto.SoldCount);
            _dataContext.SellFactors.Should()
                .Contain(_ => _.DateSold == DateTime.Now.Date);
            _dataContext.SellFactors.Should()
                .Contain(_ => _.Count == dto.SoldCount);
        }

        [Fact]
        public void Sell_throws_exception_NotEnoughProductException_if_thres_not_enough_products_in_inventory()
        {
            var product = ProductFactory.GenerateProductWithCategory("Test",4,3);
            _dataContext.Manipulate(_ => _.Products.Add(product));
            var dto = ProductFactory.GenerateSellProductDto(22);

            Action expected = () => _sut.Sell(product.Id, dto);

            expected.Should().ThrowExactly<NotEnoughProductException>();
        }

        [Fact]
        public void Sell_throws_exception_ProductNotFoundException_if_product_dosnt_exist()
        {
            var fakeProductId = 23;
            var dto = ProductFactory.GenerateSellProductDto(2);

            Action expected = () => _sut.Sell(fakeProductId, dto);

            expected.Should().ThrowExactly<ProductNotFoundException>();
        }

        [Fact]
        public void Buy_buys_the_Product_properly()
        {
            var product = ProductFactory.GenerateProductWithCategory("test",3);
            var count = product.Count;
            _dataContext.Manipulate(_ => _.Products.Add(product));
            var dto = ProductFactory.GenerateBuyProductDto(2);

            _sut.Buy(product.Id, dto);

            _dataContext.BuyFactors.Should().HaveCount(1);
            _dataContext.Products.Should()
                .Contain(_ => _.Count == count + dto.BoughtCount);
            _dataContext.BuyFactors.Should()
                .Contain(_ => _.DateBought == DateTime.Now.Date);
            _dataContext.BuyFactors.Should()
                .Contain(_ => _.Count == dto.BoughtCount);
        }

        [Fact]
        public void Buy_throws_exception_ProductNotFoundException_if_product_dosnt_exist()
        {
            var fakeProductId = 23;
            var dto = ProductFactory.GenerateBuyProductDto(2);

            Action expected = () => _sut.Buy(fakeProductId, dto);

            expected.Should().ThrowExactly<ProductNotFoundException>();
        }
    }
}
