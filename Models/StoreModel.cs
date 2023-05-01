using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCommerce.Models
{
    [NotMapped]
    public class Store
	{

		public List<Product> Products { get; set; }

        
        public string? Query { get; set; }

        public string? Category { get; set; }
    }
}

