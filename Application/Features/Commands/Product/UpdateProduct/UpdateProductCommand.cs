using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using MediatR;

namespace Application.Features.Commands.Product.UpdateProduct
{
    public record  UpdateProductCommand(ProductDTO? Product) : IRequest<ProductDTO?>;
}
