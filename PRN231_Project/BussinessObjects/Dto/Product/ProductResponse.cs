using BussinessObjects.Dto.Album;
using BussinessObjects.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Dto.Product
{
	public class ProductResponse : PagingResponsse
	{
		public List<UpdateProductDto>? Product { get; set; }
	}
}
