using FluentAssertions;
using StoreBDD.Infrastructure.Application;
using StoreBDD.Infrastructure.Test;
using StoreBDD.Persistence.EF;
using StoreBDD.Persistence.EF.Products;
using StoreBDD.Persistence.EF.SellFactors;
using StoreBDD.Services.Products;
using StoreBDD.Services.Products.Contracts;
using StoreBDD.Services.Products.Exceptions;
using StoreBDD.Services.SellFactors.Contracts;
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
        private readonly UnitOfWork _unitOfWork;
        private readonly ProductService _sut;
        private readonly ProductRepository _repository;
        private readonly SellFactorRepository _sellRepository;
        public ProductServiceTest()
        {
            _dataContext =
                new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFProductRepository(_dataContext);
            _sellRepository = new EFSellFactorRepository(_dataContext);
            _sut = new ProductAppService(_repository, 
                _unitOfWork,_sellRepository);
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
            var product = ProductFactory.GenerateProduct("Test", category.Id);
            _dataContext.Manipulate(_ => _.Products.Add(product));
            var dto = ProductFactory
                .GenerateAddProductDto(product.Name, category.Id);

            Action expected =()=>  _sut.Add(dto);

            expected.Should()
                .ThrowExactly<DuplicateProductNameInSameCategoryException>();
        }

        [Fact]
        public void Update_updates_the_Product_properly()
        {
            var category = CategoryFactory.GenerateCategory("DummyTitle");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var product = ProductFactory.GenerateProduct("Test", category.Id);
            _dataContext.Manipulate(_ => _.Products.Add(product));
            var dto = ProductFactory
                .GenerateUpdateProductDto("UpdatedName", category.Id);

            _sut.Update(product.Id,dto);

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
            var product = ProductFactory.GenerateProduct("Test", category.Id);
            var product2 = ProductFactory.GenerateProduct("Test2", category.Id);
            _dataContext.Manipulate(_ => _.Products.AddRange(product,product2));
            var dto = ProductFactory
                .GenerateUpdateProductDto(product2.Name, category.Id);

            Action expected =()=> _sut.Update(product.Id, dto);

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

            Action expected =()=> _sut.Update(fakeProductId, dto);

            expected.Should().ThrowExactly<ProductNotFoundException>();
        }

        [Fact]
        public void Delete_deletes_the_Product_properly()
        {
            var category = CategoryFactory.GenerateCategory("DummyTitle");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var product = ProductFactory.GenerateProduct("Test", category.Id);
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
            var category = CategoryFactory.GenerateCategory("DummyTitle");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var product = ProductFactory.GenerateProduct("Test", category.Id);
            _dataContext.Manipulate(_ => _.Products.Add(product));

            var expected = _sut.Get(product.Id);

            expected.Name.Should().Be(product.Name);
            expected.Count.Should().Be(product.Count);
            expected.CategoryId.Should().Be(product.CategoryId);
            expected.Price.Should().Be(product.Price);
        }
    }
}
