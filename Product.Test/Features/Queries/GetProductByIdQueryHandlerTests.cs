using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions.Product;
using Application.Features.Queries.Product.GetProduct;
using Application.Features.Queries.Product.GetProductById;
using Application.Interfaces;
using Application.Models;
using Application.Util;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;

namespace Product.Test.Features.Queries
{
    public class GetProductByIdQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly GetProductsByIdQueryHandler _handler;
        private readonly GetProductByIdQueryValidator _validator;

        public GetProductByIdQueryHandlerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _handler = new GetProductsByIdQueryHandler(_mockRepository.Object);
            _validator = new();
        }

        [Fact]
        public async Task Handle_ProductExists_ReturnsProduct()
        {
            // Arrange
            var productId = 1;
            var expectedProduct = new ProductDTO { Id = productId };

            _mockRepository.Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedProduct);

            var query = new GetProductByIdQuery(productId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeSameAs(expectedProduct);
            _mockRepository.Verify(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()), Times.Once);
        }
            
        [Fact]
        public async Task Handle_ProductDoesNotExist_ThrowsDataNotFoundException()
        {
            // Arrange
            var invalidProductId =1222;
            _mockRepository.Setup(r => r.GetByIdAsync(invalidProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductDTO)null);

            var query = new GetProductByIdQuery(invalidProductId);

            // Act & Assert
            await _handler.Invoking(h => h.Handle(query, CancellationToken.None))
                .Should().ThrowAsync<ProductException>()
                .WithMessage(ProductMessageException.DataNotFound.GetDescription());
        }

        [Fact]
        public void Validate_IdIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var query = new GetProductByIdQuery(null); 

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Id can not be null");
        }

        [Fact]
        public void Validate_ValidId_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var query = new GetProductByIdQuery(1);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
