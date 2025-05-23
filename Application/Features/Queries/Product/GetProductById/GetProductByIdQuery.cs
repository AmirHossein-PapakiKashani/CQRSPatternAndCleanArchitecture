﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using MediatR;

namespace Application.Features.Queries.Product.GetProduct
{
    public record GetProductByIdQuery(int? Id) : IRequest<ProductDTO>;
}
