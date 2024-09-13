using AutoFixture;
using FluentAssertions;
using NSubstitute;
using refactor_this.DataAccess;
using refactor_this.Models;
using refactor_this.Services;
using System.Data.SqlClient;

namespace RefactorThis.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly IDataAccessLayer _dataAccessLayer;
        private readonly IProductOptionService _productOptionService;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _dataAccessLayer = Substitute.For<IDataAccessLayer>();
            _productOptionService = Substitute.For<IProductOptionService>();
            _productService = new ProductService(_dataAccessLayer, _productOptionService);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            // Arrange
            var products = new Products(new Fixture().CreateMany<Product>().ToList());
            _dataAccessLayer.ExecuteQueryAsync<Product>(Arg.Any<string>(), Arg.Any<SqlParameter[]>(), Arg.Any<Func<SqlDataReader, Product>>())
                .Returns(products.Items);

            // Act
            var result = await _productService.GetAllAsync();

            // Assert
            result.Should().BeEquivalentTo(products);
        }

        [Fact]
        public async Task SearchByNameAsync_WithValidName_ShouldReturnMatchingProducts()
        {
            // Arrange
            var name = "test";
            var products = new Products(new Fixture().CreateMany<Product>().ToList());
            _dataAccessLayer.ExecuteQueryAsync<Product>(Arg.Any<string>(), Arg.Any<SqlParameter[]>(), Arg.Any<Func<SqlDataReader, Product>>())
                .Returns(products.Items);

            // Act
            var result = await _productService.SearchByNameAsync(name);

            // Assert
            result.Should().BeEquivalentTo(products);
        }

        [Fact]
        public async Task SearchByNameAsync_WithNullOrEmptyName_ShouldThrowArgumentNullException()
        {
            // Arrange
            string name = string.Empty;

            // Act
            Func<Task> act = async () => await _productService.SearchByNameAsync(name);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("name cannot be null or empty (Parameter 'name')");
        }

        [Fact]
        public async Task GetProductAsync_WithValidId_ShouldReturnProduct()
        {
            // Arrange
            var id = Guid.NewGuid();
            var product = new Fixture().Create<Product>();
            _dataAccessLayer.ExecuteQueryAsync<Product>(Arg.Any<string>(), Arg.Any<SqlParameter[]>(), Arg.Any<Func<SqlDataReader, Product>>())
                .Returns(new[] { product }.ToList());

            // Act
            var result = await _productService.GetProductAsync(id);

            // Assert
            result.Should().BeEquivalentTo(product);
        }

        [Fact]
        public async Task GetProductAsync_WithEmptyId_ShouldThrowArgumentException()
        {
            // Arrange
            var id = Guid.Empty;

            // Act
            Func<Task> act = async () => await _productService.GetProductAsync(id);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("id (Parameter 'id cannot be empty')");
        }

        [Fact]
        public async Task SaveAsync_WithNewProduct_ShouldInsertProduct()
        {
            // Arrange
            var product = new Fixture().Create<Product>();

            // Act
            await _productService.SaveAsync(product);

            // Assert
            await _dataAccessLayer.Received(1).ExecuteNonQueryAsync(Arg.Any<string>(), Arg.Any<SqlParameter[]>());
        }

        [Fact]
        public async Task SaveAsync_WithExistingProduct_ShouldUpdateProduct()
        {
            // Arrange
            var product = new Fixture().Create<Product>();

            // Act
            await _productService.SaveAsync(product, true);

            // Assert
            await _dataAccessLayer.Received(1).ExecuteNonQueryAsync(Arg.Any<string>(), Arg.Any<SqlParameter[]>());
        }

        [Fact]
        public async Task UpdateAsync_WithValidId_ShouldUpdateProduct()
        {
            // Arrange
            var id = Guid.NewGuid();

            var fixture = new Fixture();
            fixture.Register(() => new Product(false));

            Product product = fixture.Create<Product>();

            product.Id = id;

            _dataAccessLayer.ExecuteQueryAsync<Product>(Arg.Any<string>(), Arg.Any<SqlParameter[]>(), Arg.Any<Func<SqlDataReader, Product>>())
                .Returns(new Product[] { product }.ToList());

            _dataAccessLayer.ExecuteNonQueryAsync(Arg.Any<string>(), Arg.Any<SqlParameter[]>())
                .Returns(1);

            // Act
            await _productService.UpdateAsync(id, product);

            // Assert
            await _dataAccessLayer.Received(1).ExecuteNonQueryAsync(Arg.Any<string>(), Arg.Any<SqlParameter[]>());
        }

        [Fact]
        public async Task UpdateAsync_WithEmptyId_ShouldThrowArgumentException()
        {
            // Arrange
            var id = Guid.Empty;
            var product = new Fixture().Create<Product>();
            product.Id = id;

            // Act
            Func<Task> act = async () => await _productService.UpdateAsync(id, product);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("id (Parameter 'id cannot be empty')");
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_ShouldDeleteProductAndOptions()
        {
            // Arrange
            var id = Guid.NewGuid();
            var options = new ProductOptions(new Fixture().CreateMany<ProductOption>().ToList());
            _productOptionService.GetOptionsAsync(id).Returns(options);

            // Act
            await _productService.DeleteAsync(id);

            // Assert
            await _productOptionService.Received(options.Items.Count).DeleteAsync(Arg.Any<Guid>());
            await _dataAccessLayer.Received(1).ExecuteNonQueryAsync(Arg.Any<string>(), Arg.Any<SqlParameter[]>());
        }

        [Fact]
        public async Task DeleteAsync_WithEmptyId_ShouldThrowArgumentException()
        {
            // Arrange
            var id = Guid.Empty;

            // Act
            Func<Task> act = async () => await _productService.DeleteAsync(id);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("id (Parameter 'id cannot be empty')");
        }
    }
}
