using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions.Product;
using Application.Features.Commands.Product.UpdateProduct;
using Application.Interfaces;
using Application.Models;
using Application.Util;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;

namespace Product.Test.Features.Commands.Product
{

    public class UpdateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly UpdateProductCommandHandler _handler;
        private readonly UpdateProductCommandValidator _validator;
        public UpdateProductCommandHandlerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _handler = new UpdateProductCommandHandler(_mockRepository.Object);
            _validator = new();
        }

        [Fact]
        public async Task Handle_ProductExists_UpdatesAndReturnsProduct()
        {
            // Arrange
            var productId = 1;
            var updatedProduct = new ProductDTO { Id = productId, Title = "New Title" };

            _mockRepository.Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedProduct);
            _mockRepository.Setup(r => r.UpdateAsync(updatedProduct, It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedProduct);

            var command = new UpdateProductCommand(updatedProduct);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeSameAs(updatedProduct);
            _mockRepository.Verify(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()), Times.Once);
            _mockRepository.Verify(r => r.UpdateAsync(updatedProduct, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ProductDoesNotExist_ThrowsDataNotFoundException()
        {
            // Arrange
            var invalidProduct = new ProductDTO { Id = 122 };
            _mockRepository.Setup(r => r.GetByIdAsync(invalidProduct.Id.Value, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductDTO)null);

            var command = new UpdateProductCommand(invalidProduct);

            // Act & Assert
            await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<ProductException>()
                .WithMessage(ProductMessageException.DataNotFound.GetDescription());

            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<ProductDTO>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_UpdateReturnsNull_ReturnsNull()
        {
            // Arrange
            var productId = 1;
            var product = new ProductDTO { Id = productId };

            _mockRepository.Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);
            _mockRepository.Setup(r => r.UpdateAsync(product, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductDTO)null);

            var command = new UpdateProductCommand(product);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Validate_ProductIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateProductCommand(null); 

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Product)
                .WithErrorMessage("Product detail can not be null");

            result.ShouldNotHaveValidationErrorFor(x => x.Product.Title); 
            result.ShouldNotHaveValidationErrorFor(x => x.Product.Id);    
        }

        [Fact]
        public void Validate_TitleIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var product = new ProductDTO { Id = 1, Title = null! };
            var command = new UpdateProductCommand(product);


            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Product!.Title)
                .WithErrorMessage("Title must be specified");
        }

        [Fact]
        public void Validate_IdIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var product = new ProductDTO { Id = null, Title = "Valid Title" };
            var command = new UpdateProductCommand(product);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Product.Id)
                .WithErrorMessage("Id can not be null");
        }

        [Fact]
        public void Validate_ValidCommand_ShouldNotHaveErrors()
        {
            // Arrange
            var product = new ProductDTO { Id = 12, Title = "Valid Title" };
            var command = new UpdateProductCommand(product);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
