using FluentAssertions;
using StoreBDD.Entities;
using StoreBDD.Infrastructure.Application;
using StoreBDD.Infrastructure.Test;
using StoreBDD.Persistence.EF;
using StoreBDD.Persistence.EF.Products;
using StoreBDD.Services.Products;
using StoreBDD.Services.Products.Contracts;
using StoreBDD.Specs.Infrastructure;
using System.Linq;
using Xunit;
using static StoreBDD.Specs.BDDHelper;

namespace StoreBDD.Specs.Products
{
    [Scenario("مدیریت کالا")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = " کالا های خود را مدیریت کنم",
        InOrderTo = "کالا های خود را بفروشم"
        )]
    public class AddProduct : EFDataContextDatabaseFixture
    {
        private readonly ProductService _sut;
        private readonly UnitOfWork _unitOfWork;
        private readonly ProductRepository _repository;
        private readonly EFDataContext _dataContext;
        private AddProductDto _dto;
        private Category _category;

        public AddProduct(ConfigurationFixture configuration) : base(
            configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFProductRepository(_dataContext);
            _sut = new ProductAppService(_repository, _unitOfWork);
        }

        [Given("دسته بندی با عنوان 'لبنیات'در فهرست دسته بندی کالا وجود دارد")]
        public void Given()
        {
            _category = new Category
            {
                Title = "لبنیات"
            };

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [And("هیچ کالایی در دسته بندی 'لبنیات' وجود ندارد")]
        public void GivenAnd()
        {

        }

        [When("کالایی با عنوان 'ماست کاله' و قیمت '5000' و حداقل موجودی '5' به دسته بندی 'لبنیات' اضافه میکنیم")]
        public void When()
        {
            _dto = new AddProductDto
            {
                Name = "ماست کاله",
                MinimumCount = 5,
                Price = 5000,
                CategoryId = _category.Id
            };

            _sut.Add(_dto);
        }

        [Then("کالایی با عنوان 'ماست کاله' و قیمت '5000' و تعداد '0' در دسته بندی 'لبنیات' باید وجود داشته باشد")]
        public void Then()
        {
            _dataContext.Products.Count().Should().Be(1);
            _dataContext.Products
                .Should().Contain(_ => _.Name == _dto.Name);
            _dataContext.Products
                .Should().Contain(_ => _.CategoryId == _dto.CategoryId);
            _dataContext.Products
                .Should().Contain(_ => _.Price == _dto.Price);
            _dataContext.Products
                .Should().Contain(_ => _.MinimumCount == _dto.MinimumCount);

        }

        [Fact]
        public void Run()
        {
            Given();
            GivenAnd();
            When();
            Then();
        }
    }
}
