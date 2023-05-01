using System;
using System.ComponentModel.DataAnnotations;

namespace GameCommerce.Models
{
	public class CartObject
	{
		[Key]
		public int Id { get; set; }

		public Product? Product { get; set; }

		public int Quantity { get; set; }


	}
}

