using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{

    public class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the product mapping profile
        /// </summary>
        /// <remarks>
        /// Configures bidirectional mapping between:
        /// <para>- <see cref="Product"/> (domain entity) </para>
        /// <para>- <see cref="ProductDTO"/> (data transfer object) </para>
        /// </remarks>
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ReverseMap();
        }
    }
}
