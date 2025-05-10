using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Queries.Product.GetProduct;
using FluentValidation;

namespace Application.Features.Queries.Product.GetProductById
{
    public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
    {
        public GetProductByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id can not be empty")
                .NotNull().WithMessage("Id can not be null");
        }
    }
}
