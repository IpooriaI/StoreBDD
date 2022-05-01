using FluentAssertions;
using StoreBDD.Entities;
using StoreBDD.Infrastructure.Application;
using StoreBDD.Infrastructure.Test;
using StoreBDD.Persistence.EF;
using StoreBDD.Persistence.EF.Categories;
using StoreBDD.Services.Categories;
using StoreBDD.Services.Categories.Contracts;
using StoreBDD.Services.Categories.Exceptions;
using StoreBDD.Test.Tools.Categories;
using StoreBDD.Test.Tools.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StoreBDD.Services.Test.Unit.Categories
{
    public class CategoryServiceTests 
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryService _sut;
        private readonly CategoryRepository _repository;

        public CategoryServiceTests()
        {
            _dataContext =
                new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void Add_adds_category_properly()
        {
            AddCategoryDto dto = CategoryFactory
                .GenerateAddCategoryDto("Dummy");

            _sut.Add(dto);

            _dataContext.Categories.Should().HaveCount(1);
            _dataContext.Categories.Should().Contain(_ => _.Title == dto.Title);
        }

        [Fact]
        public void Add_throws_DuplicateCategoryTitle_when_category_with_the_same_title_already_exists()
        {
            var category = CategoryFactory.GenerateCategory("DummyTitle");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var dto = CategoryFactory.GenerateAddCategoryDto(category.Title);

            Action expected =()=> _sut.Add(dto);

            expected.Should().ThrowExactly<DuplicateCategoryTitleException>();
        }

        [Fact]
        public void Update_updates_the_category_properly()
        {
            var category = CategoryFactory.GenerateCategory("DummyTitle");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var dto = CategoryFactory.GenerateUpdateCategoryDto("UpdatedTitle");

            _sut.Update(category.Id, dto);

            var excpected = _dataContext.Categories
                .FirstOrDefault(_ => _.Id == category.Id);
            excpected.Title.Should().Be(dto.Title);
        }

        [Fact]
        public void Update_throws_DuplicateCategoryTitle_when_category_with_the_same_title_already_exists()
        {
            var category = CategoryFactory.GenerateCategory("DummyTitle");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var SecondCategory = CategoryFactory.GenerateCategory("DummyTitle2");
            _dataContext.Manipulate(_ => _.Categories.Add(SecondCategory));
            var dto = CategoryFactory.GenerateUpdateCategoryDto(category.Title);

            Action expected = () => _sut.Update(SecondCategory.Id,dto);

            expected.Should().ThrowExactly<DuplicateCategoryTitleException>();
        }

        [Fact]
        public void Update_throws_exception_CategoryNotFoundException_if_category_dosnt_exist()
        {
            var fakeCategoryId = 20;
            var dto = CategoryFactory.GenerateUpdateCategoryDto("TestTitle");

            Action expected = () => _sut.Update(fakeCategoryId, dto);

            expected.Should().ThrowExactly<CategoryNotFoundException>();
        }

        [Fact]
        public void Delete_deletes_category_properly()
        {
            var category = CategoryFactory.GenerateCategory("DummyTitle");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            _sut.Delete(category.Id);

            _dataContext.Categories.Count().Should().Be(0);
            _dataContext.Categories.Should().NotContain(category);
        }

        [Fact]
        public void Delete_throws_exception_CategoryNotFoundException_if_category_dosnt_exist()
        {
            var fakeCategoryId = 20;
            var dto = CategoryFactory.GenerateUpdateCategoryDto("TestTitle");

            Action expected = () => _sut.Delete(fakeCategoryId);

            expected.Should().ThrowExactly<CategoryNotFoundException>();
        }

        [Fact]
        public void Delete_throws_exception_CategoryHasProductsException_if_category_has_products_inside()
        {
            var category = CategoryFactory.GenerateCategory("DummyTitle");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var product = ProductFactory.GenerateProduct("Test",category.Id);
            _dataContext.Manipulate(_ => _.Products.Add(product));

            Action expected = () => _sut.Delete(category.Id);

            expected.Should().ThrowExactly<CategoryHasProductsException>();
        }

        [Fact]
        public void Get_returns_a_category_and_its_products_properly()
        {
            var category = CategoryFactory.GenerateCategory("DummyTitle");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var product = ProductFactory.GenerateProduct("Test", category.Id);
            _dataContext.Manipulate(_ => _.Products.Add(product));

            var expected = _sut.Get(category.Id);

            expected.Title.Should().Be(category.Title);
            expected.Products[0].Name.Should().Be(product.Name);
        }

        [Fact]
        public void GetAll_returns_all_categories_and_thier_products_properly()
        {
            var category = CategoryFactory.GenerateCategory("DummyTitle");
            var category2 = CategoryFactory.GenerateCategory("DummyTitle2");
            _dataContext.Manipulate(_ => _.Categories
                .AddRange(category,category2));
            var product = ProductFactory.GenerateProduct("Test", category.Id);
            var product2 = ProductFactory.GenerateProduct("Test2", category2.Id);
            _dataContext.Manipulate(_ => _.Products.AddRange(product,product2));

            var expected = _sut.GetAll();

            expected.Should().Contain(_ => _.Title == category.Title);
            expected.Should().Contain(_ => _.Title == category2.Title);
        }
    }
}
