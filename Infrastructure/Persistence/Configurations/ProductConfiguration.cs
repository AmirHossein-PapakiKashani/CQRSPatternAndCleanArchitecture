using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configures the database model for the <see cref="Product"/> entity
    /// </summary>
    /// <remarks>
    /// Implements <see cref="IEntityTypeConfiguration{TEntity}"/> to define the database schema
    /// for products. This configuration is typically applied in the <see cref="DbContext.OnModelCreating"/>
    /// method using <c>modelBuilder.ApplyConfiguration()</c>.
    /// </remarks>
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        /// <summary>
        /// Configures the entity model for the <see cref="Product"/> type
        /// </summary>
        /// <param name="builder">The builder used to construct the model for this entity type</param>
        /// <remarks>
        /// This configuration specifies:
        /// <para>1. <see cref="Product.Id"/> as the primary key with identity column</para>
        /// <para>2. <see cref="Product.Title"/> as a required property</para>
        /// </remarks>
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                .Property(x => x.Id)
                .UseIdentityColumn()
                .IsRequired();

            builder
                .HasKey(x => x.Id);

            builder
              .Property(x => x.Title)
              .IsRequired(true);

        }
    }
}
