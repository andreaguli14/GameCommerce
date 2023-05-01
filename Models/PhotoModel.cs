using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCommerce.Models
{
	public class Photo
	{
		[Key]
		public int Id { get; set; }

		public string Path { get; set; }

		public bool main { get; set; } = false;

    }
}

