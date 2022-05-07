using FluentAssertions;
using StoreBDD.Infrastructure.Test;
using StoreBDD.Persistence.EF;
using StoreBDD.Persistence.EF.SellFactors;
using StoreBDD.Services.SellFactors;
using StoreBDD.Services.SellFactors.Contracts;
using StoreBDD.Services.SellFactors.Exceptions;
using StoreBDD.Test.Tools.SellFactors;
using System;
using Xunit;

namespace StoreBDD.Services.Test.Unit.SellFactors
{
    public class SellFactorServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly SellFactorService _sut;

        public SellFactorServiceTests()
        {
            _dataContext = new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            var _unitOfWork = new EFUnitOfWork(_dataContext);
            var _repository = new EFSellFactorRepository(_dataContext);
            _sut = new SellFactorAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void GetAll_Returns_all_GetSellFactorDto_properly()
        {
            var sellFactor = SellFactorFactory.GenerateSellFactor();
            _dataContext.Manipulate(_ => _.SellFactors.Add(sellFactor));

            var expected = _sut.GetAll();

            expected.Should().Contain(_ => _.ProductId == sellFactor.ProductId);
            expected.Should().Contain(_ => _.DateSold == DateTime.Now.Date);
            expected.Should().Contain(_ => _.Count == sellFactor.Count);
        }

        [Fact]
        public void Get_Returns_a_GetSellFactorDto_properly()
        {
            var sellFactor = SellFactorFactory.GenerateSellFactor();
            _dataContext.Manipulate(_ => _.SellFactors.Add(sellFactor));

            var expected = _sut.Get(sellFactor.Id);

            expected.Id.Should().Be(sellFactor.Id);
            expected.Count.Should().Be(sellFactor.Count);
            expected.ProductId.Should().Be(sellFactor.ProductId);
        }

        [Fact]
        public void Delete_deletes_the_factor_properly()
        {
            var sellFactor = SellFactorFactory.GenerateSellFactor();
            _dataContext.Manipulate(_ => _.SellFactors.Add(sellFactor));

            _sut.Delete(sellFactor.Id);

            _dataContext.SellFactors.Should().HaveCount(0);
            _dataContext.SellFactors.Should().NotContain(sellFactor);
        }

        [Fact]
        public void Delete_throws_BuyFactorNotFoundException_if_factor_dosnt_exist()
        {
            var fakeFactorId = 5;

            Action expected = () => _sut.Delete(fakeFactorId);

            expected.Should().ThrowExactly<SellFactorNotFoundException>();
        }

        [Fact]
        public void Update_updates_the_factor_properly()
        {
            var sellFactor = SellFactorFactory.GenerateSellFactor();
            _dataContext.Manipulate(_ => _.SellFactors.Add(sellFactor));
            var dto = SellFactorFactory
                .GenerateUpdateSellFactorDto(3, DateTime.Now);

            _sut.Update(sellFactor.Id, dto);

            _dataContext.SellFactors.Should().HaveCount(1);
            _dataContext.SellFactors.Should().Contain(_ => _.Count == dto.Count);
            _dataContext.SellFactors.Should()
                .Contain(_ => _.DateSold == dto.DateSold);
        }

        [Fact]
        public void Update_throws_BuyFactorNotFoundException_if_factor_dosnt_exist()
        {
            var fakeFactorId = 5;
            var dto = SellFactorFactory
                .GenerateUpdateSellFactorDto(1, DateTime.Now);

            Action expected = () => _sut.Update(fakeFactorId, dto);

            expected.Should().ThrowExactly<SellFactorNotFoundException>();
        }
    }
}
