using BussinessObjects.Dto.Product;
using BussinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ProductRepo
{
    public interface IProductRepository
    {
		Task<ProductResponse> GetProduct(ProductRequest request);
        Product GetProductById(int id);
		List<Product> GetProductByCategoryId(int id);



	}
}
