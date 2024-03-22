using BussinessObjects.Paging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Dto.Product
{
	public class ProductRequest : PagingRequestBase
	{	
		public int CategoryId { get; set; }
		public int PriceFrom { get; set; }
		public int PriceTo { get; set; }
	}
}
