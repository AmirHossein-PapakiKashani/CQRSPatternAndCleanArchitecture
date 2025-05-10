using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Commands.Product.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            
            RuleFor(x => x.Product)
                .NotNull().WithMessage("Instance can not be null")
                .ChildRules(product =>
                {
                    product.RuleFor(p => p.Title)
                        .NotNull()
                        .NotEmpty().WithMessage("Insert title is required.")
                        .MaximumLength(20).WithMessage("Title is invalid");
                });
        }
    }
}
