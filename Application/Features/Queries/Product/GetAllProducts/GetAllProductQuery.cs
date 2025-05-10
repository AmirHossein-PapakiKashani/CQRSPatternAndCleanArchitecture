using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Models;
using MediatR;

namespace Application.Features.Queries.Product.GetAllProducts
{
    public record GetAllProductQuery : IRequest<List<ProductDTO?>>;  
}
