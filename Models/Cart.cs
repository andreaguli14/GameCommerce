using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCommerce.Models
{
	public class Cart
	{
		[Key]
		public int Id { get; set; }

		public List<CartObject> Objects { get; set; } = new List<CartObject>();

        [NotMapped]
        public string? Coupon { get; set; }
	}
}

