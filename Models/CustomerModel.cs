using System;
using Microsoft.AspNetCore.Identity;

namespace GameCommerce.Models
{
	public class Customer : IdentityUser
    {
		public Cart Cart { get; set; } = new Cart();

	
	}
}

