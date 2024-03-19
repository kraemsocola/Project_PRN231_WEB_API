using AutoMapper;
using BussinessObjects.Dto.Album;
using BussinessObjects.Dto.Product;
using BussinessObjects.Helper;
using BussinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ProductRepo
{
    public class ProductRepository : IProductRepository
    {
		private readonly ProjectDbContext _context;
		private readonly IMapper _mapper;

		public ProductRepository(ProjectDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<ProductResponse> GetProduct(ProductRequest request)
		{
			try
			{
				var query = (from p in _context.Product
							 join a in _context.Album on p.Id equals a.ProductId into productAlbums
							 from pa in productAlbums.DefaultIfEmpty()
							 select new Product
							 {
								 Id = p.Id,
								 Name = p.Name,
								 Title = p.Title,
								 Description = p.Description,
								 Star = p.Star,
								 Price = p.Price,
								 Quantity = p.Quantity,
								 CategoryId = p.CategoryId,
								 Category = p.Category,
								 Thumbnail = p.Thumbnail
							 }).OrderBy(pa => pa.Id);

				List<Product> productList = await query.ToListAsync();	
				
				if (request.KeyWords != null)
				{
					productList = productList.Where(x => x.Name.Contains(request.KeyWords)).ToList();
				}				

				var productDtos = _mapper.Map<List<UpdateProductDto>>(productList).Paginate(request).ToList();


				var response = new ProductResponse
				{
					Product = productDtos,
					Page = Paging.GetPagingResponse(request, productList.Count())
				};

				return response;

			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

		}
	}
}
