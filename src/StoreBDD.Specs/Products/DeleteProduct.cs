using FluentAssertions;
using StoreBDD.Entities;
using StoreBDD.Infrastructure.Test;
using StoreBDD.Persistence.EF;
using StoreBDD.Persistence.EF.BuyFactors;
using StoreBDD.Persistence.EF.Products;
using StoreBDD.Persistence.EF.SellFactors;
using StoreBDD.Services.Products;
using StoreBDD.Services.Products.Contracts;
using StoreBDD.Specs.Infrastructure;
using StoreBDD.Test.Tools.Categories;
using StoreBDD.Test.Tools.Products;
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
    public class DeleteProduct : EFDataContextDatabaseFixture
    {
        private readonly ProductService _sut;
        private readonly EFDataContext _dataContext;
        private Category _category;
        private Product _product;

        public DeleteProduct(ConfigurationFixture configuration) : base(
            configuration)
        {
            _dataContext = CreateDataContext();
            var _unitOfWork = new EFUnitOfWork(_dataContext);
            var _repository = new EFProductRepository(_dataContext);
            var _sellRepository = new EFSellFactorRepository(_dataContext);
            var _buyRepository = new EFBuyFactorRepository(_dataContext);
            _sut = new ProductAppService(_repository,
                _unitOfWork, _sellRepository, _buyRepository);
        }

        [Given("دسته بندی با عنوان 'لبنیات'در فهرست دسته بندی کالا وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.GenerateCategory("لبنیات");

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [And("کالایی با عنوان 'ماست کاله' و قیمت '5000' و تعداد '20' و حداقل موجودی '5' در دسته بندی 'لبنیات' وجود دارد")]
        public void GivenAnd()
        {
            _product = ProductFactory
                .GenerateProduct("ماست کاله", _category.Id,3);

            _dataContext.Manipulate(_ => _.Products.Add(_product));
        }

        [When("کالایی با عنوان 'ماست کاله' و قیمت '5000' و تعداد '20' و حداقل موجودی '5' از دسته بندی 'لبنیات' حذف میکنیم")]
        public void When()
        {
            _sut.Delete(_product.Id);
        }

        [Then("کالایی با عنوان 'ماست کاله' و قیمت '5000' و تعداد '0' در دسته بندی 'لبنیات' باید وجود داشته باشد")]
        public void Then()
        {
            _dataContext.Products.Count().Should().Be(0);
            _dataContext.Products
                .Should().NotContain(_ => _.Name == _product.Name);
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
