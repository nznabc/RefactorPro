using AutoFixture;
using FluentAssertions;
using NSubstitute;
using refactor_this.DataAccess;
using refactor_this.Models;
using refactor_this.Services;
using System.Data.SqlClient;

namespace RefactorThis.Tests.Services
{
    public class ProductOptionServiceTests
    {
        private readonly IDataAccessLayer _dataAccessLayer;
        private readonly ProductOptionService _productOptionService;
        private readonly Fixture _fixture;

        public ProductOptionServiceTests()
        {
            _dataAccessLayer = Substitute.For<IDataAccessLayer>();
            _productOptionService = new ProductOptionService(_dataAccessLayer);
            _fixture = new Fixture();
        }

        private void SetupDataAccessLayerToReturnResult<T>(List<T> result)
        {
            _dataAccessLayer.ExecuteQueryAsync<T>(Arg.Any<string>(), Arg.Any<SqlParameter[]>(), Arg.Any<Func<SqlDataReader, T>>())
                .Returns(result);
        }

        [Fact]
        public async Task GetOptionsAsync_WithValidId_ShouldReturnProductOptions()
        {
            // Arrange
            var productId = _fixture.Create<Guid>();

            var productOptions = new ProductOptions(_fixture.CreateMany<ProductOption>().ToList());

            SetupDataAccessLayerToReturnResult(productOptions.Items);

            // Act
            var result = await _productOptionService.GetOptionsAsync(productId);

            // Assert
            result.Should().BeEquivalentTo(productOptions);
        }

        [Fact]
        public async Task GetOptionsAsync_WithEmptyId_ShouldThrowArgumentException()
        {
            // Arrange
            var productId = Guid.Empty;

            // Act
            Func<Task> act = async () => await _productOptionService.GetOptionsAsync(productId);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage("id cannot be empty");
        }

        [Fact]
        public async Task GetOptionAsync_WithValidId_ShouldReturnProductOption()
        {
            // Arrange
            var productId = _fixture.Create<Guid>();
            var productOption = _fixture.Create<ProductOption>();

            SetupDataAccessLayerToReturnResult(new List<ProductOption> { productOption });

            // Act
            var result = await _productOptionService.GetOptionAsync(productId);

            // Assert
            result.Should().BeEquivalentTo(productOption);
        }

        [Fact]
        public async Task GetOptionAsync_WithEmptyId_ShouldThrowArgumentException()
        {
            // Arrange
            var productId = Guid.Empty;

            // Act
            Func<Task> act = async () => await _productOptionService.GetOptionAsync(productId);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage("id cannot be empty");
        }

        [Fact]
        public async Task SaveAsync_WithValidProductOption_ShouldInsertProductOption()
        {
            // Arrange
            var productId = _fixture.Create<Guid>();
            var productOption = _fixture.Create<ProductOption>();

            // Act
            await _productOptionService.SaveAsync(productId, productOption);

            // Assert
            await _dataAccessLayer.Received(1).ExecuteNonQueryAsync(Arg.Any<string>(), Arg.Any<SqlParameter[]>());
        }

        [Fact]
        public async Task UpdateAsyc_WithValidId_ShouldUpdateProductOption()
        {
            // Arrange
            var id = _fixture.Create<Guid>();
            var productId = _fixture.Create<Guid>();

            _fixture.Register(() => new ProductOption(false));

            var productOption = _fixture.Create<ProductOption>();

            productOption.Id = id;
            productOption.ProductId = productId;

            SetupDataAccessLayerToReturnResult(new List<ProductOption> { productOption });

            _dataAccessLayer.ExecuteNonQueryAsync(Arg.Any<string>(), Arg.Any<SqlParameter[]>())
                .Returns(1);

            // Act
            await _productOptionService.UpdateAsync(id, productOption);

            // Assert
            await _dataAccessLayer.Received(1).ExecuteNonQueryAsync(Arg.Any<string>(), Arg.Any<SqlParameter[]>());
        }

        [Fact]
        public async Task UpdateAsync_WithEmptyId_ShouldThrowArgumentException()
        {
            // Arrange
            var id = Guid.Empty;
            var productOption = _fixture.Create<ProductOption>();
            productOption.Id = id;

            // Act
            Func<Task> act = async () => await _productOptionService.UpdateAsync(id, productOption);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage("id cannot be empty");
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_ShouldDeleteProductOption()
        {
            // Arrange
            var id = _fixture.Create<Guid>();

            // Act
            await _productOptionService.DeleteAsync(id);

            // Assert
            await _dataAccessLayer.Received(1).ExecuteNonQueryAsync(Arg.Any<string>(), Arg.Any<SqlParameter[]>());
        }

        [Fact]
        public async Task DeleteAsync_WithEmptyId_ShouldThrowArgumentException()
        {
            // Arrange
            var id = Guid.Empty;

            // Act
            Func<Task> act = async () => await _productOptionService.DeleteAsync(id);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage("id cannot be empty");
        }
    }
}