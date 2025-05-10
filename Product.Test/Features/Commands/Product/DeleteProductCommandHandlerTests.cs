using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions.Product;
using Application.Features.Commands.Product.DeleteProduct;
using Application.Interfaces;
using Application.Models;
using Application.Util;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;

namespace Product.Test.Features.Commands.Product
{

    public class DeleteProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly DeleteProductCommandHandler _handler;
        private readonly DeleteProductCommandValidator _validator;

        public DeleteProductCommandHandlerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _handler = new DeleteProductCommandHandler(_mockRepository.Object);
            _validator = new();
        }

        [Fact]
        public async Task Handle_ProductExists_DeletesAndReturnsProduct()
        {
            // Arrange
            var productId = 1;
            var existingProduct = new ProductDTO { Id = productId };

            _mockRepository.Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);
            _mockRepository.Setup(r => r.DeleteAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);

            var command = new DeleteProductCommand(productId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeSameAs(existingProduct);
            _mockRepository.Verify(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()), Times.Once);
            _mockRepository.Verify(r => r.DeleteAsync(productId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ProductDoesNotExist_ThrowsDataNotFoundException()
        {
            // Arrange
            var invalidProductId = 100;
            _mockRepository.Setup(r => r.GetByIdAsync(invalidProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductDTO)null);

            var command = new DeleteProductCommand(invalidProductId);

            // Act & Assert
            await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<ProductException>()
                .WithMessage(ProductMessageException.DataNotFound.GetDescription());

            _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_DeleteReturnsNull_ReturnsNull()
        {
            // Arrange
            var productId = 1;
            var existingProduct = new ProductDTO { Id = productId };

            _mockRepository.Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);
            _mockRepository.Setup(r => r.DeleteAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductDTO)null);

            var command = new DeleteProductCommand(productId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
            _mockRepository.Verify(r => r.DeleteAsync(productId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Validate_ValidId_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var command = new DeleteProductCommand(2);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_IdIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = new DeleteProductCommand(null!); // Force null for test

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Id Can not be null");
        }
    }
}
