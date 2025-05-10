using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions.Product;
using Application.Features.Commands.Product.CreateProduct;
using Application.Interfaces;
using Application.Models;
using Application.Util;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;

namespace Product.Test.Features.Commands.Product
{
    public class CreateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly CreateProductCommandHandler _handler;
        private readonly CreateProductCommandValidator _validator = new();

        public CreateProductCommandHandlerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _handler = new CreateProductCommandHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_ProductExists_ThrowsProductAlreadyExistException()
        {
            // Arrange
            var productId = 1;
            var existingProduct = new ProductDTO { Id = productId };
            _mockRepository.Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);

            var command = new CreateProductCommand (new ProductDTO { Id = productId });

            // Act & Assert
            await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<ProductException>()
                .WithMessage(ProductMessageException.ProductAlreadyExist.GetDescription());
        }

        [Fact]
        public async Task Handle_AddFails_ThrowsCreateFailedException()
        {
            // Arrange
            var product = new ProductDTO { Id = 2 };
            _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductDTO)null);
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<ProductDTO>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductDTO)null);

            var command = new CreateProductCommand(product);

            // Act & Assert
            await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<ProductException>()
                .WithMessage(ProductMessageException.CreateFailed.GetDescription());

            _mockRepository.Verify(r => r.AddAsync(product, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidProduct_ReturnsProduct()
        {
            // Arrange
            var expectedProduct = new ProductDTO { Id = 3, Title = "Valid Title" };
            _mockRepository.Setup(r => r.GetByIdAsync(expectedProduct.Id!.Value, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductDTO)null);
            _mockRepository.Setup(r => r.AddAsync(expectedProduct, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedProduct);

            var command = new CreateProductCommand(expectedProduct);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeSameAs(expectedProduct);
            _mockRepository.Verify(r => r.AddAsync(expectedProduct, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Validate_ProductIsNull_ShouldHaveValidationError()
        {
            // Arrange

            var command = new CreateProductCommand(Product:null);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Product)
                .WithErrorMessage("Instance can not be null");
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Validate_TitleIsNullOrEmpty_ShouldHaveValidationError(string title)
        {
            // Arrange
            var product =  new ProductDTO { Title = title };
            var command = new CreateProductCommand(product);
            

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Product.Title)
                .WithErrorMessage(title == null ? "Insert title is required." : "Insert title is required.");
        }


        [Fact]
        public void Validate_TitleExceedsMaxLength_ShouldHaveValidationError()
        {
            // Arrange
            var product = new ProductDTO { Title = new string('a', 21) };
            var command = new CreateProductCommand(product);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Product.Title)
                .WithErrorMessage("Title is invalid");
        }


        [Fact]
        public void Validate_ValidCommand_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var product = new ProductDTO { Title = "Valid Title" , Id = 1 };
            var command = new CreateProductCommand(product);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
