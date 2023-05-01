using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCommerce.Models
{
	public class Product
	{
		[Key]
		public int Id { get; set; }

		public string Name { get; set; }

		public string? Description { get; set; }

		public string? Category { get; set; }
 
		public double Price { get; set; }

		public List<Photo>? Photos { get; set; } = new List<Photo>();

		[NotMapped]
		public List<IFormFile> ImageFile { get; set; } = new List<IFormFile>();

    }
}

