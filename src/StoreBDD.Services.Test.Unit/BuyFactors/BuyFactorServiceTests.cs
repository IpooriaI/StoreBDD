using FluentAssertions;
using StoreBDD.Infrastructure.Test;
using StoreBDD.Persistence.EF;
using StoreBDD.Persistence.EF.BuyFactors;
using StoreBDD.Services.BuyFactors;
using StoreBDD.Services.BuyFactors.Contracts;
using StoreBDD.Services.BuyFactors.Exceptions;
using StoreBDD.Test.Tools.BuyFactors;
using System;
using Xunit;

namespace StoreBDD.Services.Test.Unit.BuyFactors
{
    public class BuyFactorServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly BuyFactorService _sut;

        public BuyFactorServiceTests()
        {
            _dataContext = new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            var _unitOfWork = new EFUnitOfWork(_dataContext);
            var _repository = new EFBuyFactorRepository(_dataContext);
            _sut = new BuyFactorAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void GetAll_Returns_all_GetBuyFactorDto_properly()
        {
            var buyFactor = BuyFactorFactory.GenerateBuyFactor();
            _dataContext.Manipulate(_ => _.BuyFactors.Add(buyFactor));

            var expected = _sut.GetAll();

            expected.Should().Contain(_ => _.ProductId == buyFactor.ProductId);
            expected.Should().Contain(_ => _.DateBought == DateTime.Now.Date);
            expected.Should().Contain(_ => _.Count == buyFactor.Count);
        }

        [Fact]
        public void Get_Returns_a_GetBuyFactorDto_properly()
        {
            var buyFactor = BuyFactorFactory.GenerateBuyFactor();
            _dataContext.Manipulate(_ => _.BuyFactors.Add(buyFactor));

            var expected = _sut.Get(buyFactor.Id);

            expected.Id.Should().Be(buyFactor.Id);
            expected.ProductId.Should().Be(expected.ProductId);
            expected.Count.Should().Be(buyFactor.Count);

        }

        [Fact]
        public void Delete_deletes_the_factor_properly()
        {
            var buyFactor = BuyFactorFactory.GenerateBuyFactor();
            _dataContext.Manipulate(_ => _.BuyFactors.Add(buyFactor));

            _sut.Delete(buyFactor.Id);

            _dataContext.BuyFactors.Should().HaveCount(0);
            _dataContext.BuyFactors.Should().NotContain(buyFactor);
        }

        [Fact]
        public void Delete_throws_BuyFactorNotFoundException_if_factor_dosnt_exist()
        {
            var fakeFactorId = 5;

            Action expected = () => _sut.Delete(fakeFactorId);

            expected.Should().ThrowExactly<BuyFactorNotFoundException>();
        }

        [Fact]
        public void Update_updates_the_factor_properly()
        {
            var buyFactor = BuyFactorFactory.GenerateBuyFactor();
            _dataContext.Manipulate(_ => _.BuyFactors.Add(buyFactor));
            var dto = BuyFactorFactory
                .GenerateUpdateBuyFactorDto(3, DateTime.Now);

            _sut.Update(buyFactor.Id, dto);

            _dataContext.BuyFactors.Should().HaveCount(1);
            _dataContext.BuyFactors.Should().Contain(_ => _.Count == dto.Count);
            _dataContext.BuyFactors.Should()
                .Contain(_ => _.DateBought == dto.DateBought);
        }

        [Fact]
        public void Update_throws_BuyFactorNotFoundException_if_factor_dosnt_exist()
        {
            var fakeFactorId = 5;
            var dto = BuyFactorFactory
                .GenerateUpdateBuyFactorDto(1, DateTime.Now);

            Action expected = () => _sut.Update(fakeFactorId, dto);

            expected.Should().ThrowExactly<BuyFactorNotFoundException>();
        }
    }
}
