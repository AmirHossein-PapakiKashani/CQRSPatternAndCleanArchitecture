using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions.Product;
using Application.Features.Queries.Product.GetAllProducts;
using Application.Interfaces;
using Application.Models;
using Application.Util;
using FluentAssertions;
using Moq;

namespace Product.Test.Features.Queries
{

    public class GetAllProductQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly GetAllProductQueryHandler _handler;

        public GetAllProductQueryHandlerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _handler = new GetAllProductQueryHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_RepositoryReturnsProducts_ReturnsProductList()
        {
            // Arrange
            var expectedProducts = new List<ProductDTO?>
        {
            new ProductDTO { Id = 1, Title = "Product 1" },
            new ProductDTO { Id = 2, Title = "Product 2" }
        };

            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedProducts);

            var query = new GetAllProductQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedProducts);
            _mockRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_RepositoryReturnsNull_ThrowsDataNotFoundException()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync((List<ProductDTO?>)null!);

            var query = new GetAllProductQuery();

            // Act & Assert
            await _handler.Invoking(h => h.Handle(query, CancellationToken.None))
                .Should().ThrowAsync<ProductException>()
                .WithMessage(ProductMessageException.DataNotFound.GetDescription());

            _mockRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_RepositoryReturnsEmptyList_ReturnsEmptyList()
        {
            // Arrange
            var emptyList = new List<ProductDTO?>();
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(emptyList);

            var query = new GetAllProductQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
            _mockRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
