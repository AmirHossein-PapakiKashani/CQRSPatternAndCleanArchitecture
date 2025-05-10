using System.Reflection;
using System.Runtime.CompilerServices;
using Application.Behavior;
using Application.Features.Commands.Product.CreateProduct;
using Application.Features.Commands.Product.DeleteProduct;
using Application.Features.Commands.Product.UpdateProduct;
using Application.Features.Queries.Product.GetAllProducts;
using Application.Features.Queries.Product.GetProduct;
using Application.Interfaces;
using Application.Mappings;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SampleProject.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// Registers application layer dependencies including MediatR handlers, AutoMapper profiles, 
        /// and validation pipeline behavior for the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection to add dependencies to</param>
        /// <remarks>
        /// Configures:
        /// - MediatR handlers from multiple assemblies (commands and queries)
        /// - AutoMapper mapping profiles
        /// - Validation pipeline behavior that runs before MediatR handlers
        /// </remarks>
        public static void AddApplicationDependecies(this IServiceCollection services)
        {
            //Register MediatR
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(CreateProductCommandHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(DeleteProductCommandHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(UpdateProductCommandHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetProductsByIdQueryHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetAllProductQueryHandler).Assembly);


            });
            //Register AutoMapper
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            //Register Validator
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }

        /// <summary>
        /// Registers infrastructure layer dependencies including database context and repositories
        /// </summary>
        /// <param name="services">The service collection to add dependencies to</param>
        /// <param name="configuration">Application configuration containing connection strings</param>
        /// <remarks>
        /// Configures:
        /// - Entity Framework Core database context with SQL Server provider
        /// - Product repository implementation
        /// </remarks>
        public static void AddInfrastructureDependecies(this IServiceCollection services, IConfiguration configuration)
        {
            //Register Database
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("AppConnection")));

            //Register Repositories
            services.AddScoped<IProductRepository, ProductRepository>();

        }
    }
}
