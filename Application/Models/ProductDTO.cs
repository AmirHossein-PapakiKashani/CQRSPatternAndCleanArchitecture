using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    /// <summary>
    /// Represents a product data transfer object (DTO) used for transferring product information between layers
    public class ProductDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier for the product
        /// </summary>
        /// <value>The GUID that uniquely identifies the product</value>
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets the title/name of the product
        /// </summary>
        /// <value>The display name of the product. Defaults to empty string.</value>
        public string? Title { get; set; } = string.Empty;
    }
}
