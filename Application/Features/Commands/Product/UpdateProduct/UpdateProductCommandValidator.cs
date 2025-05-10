using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Commands.Product.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>    
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Product)
           .NotNull().WithMessage("Product detail can not be null")
           .ChildRules(product =>
           {
               product.RuleFor(p => p.Title)
                   .NotNull().WithMessage("Title must be specified");

               product.RuleFor(p => p.Id)
                   .NotNull().WithMessage("Id can not be null");
           });
        }
    }
}
