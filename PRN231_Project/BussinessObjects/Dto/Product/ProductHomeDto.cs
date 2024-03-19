using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObjects.Models;

namespace BussinessObjects.Dto.Product
{
	public class ProductHomeDto
	{
		public int Id { get; set; }

		public string? Name { get; set; }

		public string? Title { get; set; }

		public string? Description { get; set; }
		public int Star { get; set; }

		public int Price { get; set; }

		public string? Thumbnail { get; set; }

		public int Quantity { get; set; }

		public int CategoryId { get; set; }
		public Models.Category? Category { get; set; }
	}
}
