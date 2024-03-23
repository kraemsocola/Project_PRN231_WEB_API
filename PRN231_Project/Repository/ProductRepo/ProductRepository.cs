using AutoMapper;
using BussinessObjects.Dto.Album;
using BussinessObjects.Dto.Product;
using BussinessObjects.Helper;
using BussinessObjects.Models;
using BussinessObjects.Paging;
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
                
                List<UpdateProductDto> productDtos;
                ProductResponse response;
                if (request.SortType != "Popularity")
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
                        productList = productList.Where(x => x.Name.ToLower().Contains(request.KeyWords.ToLower())).ToList();
                    }
                    if (request.CategoryId != 0)
                    {
                        productList = productList.Where(x => x.CategoryId == request.CategoryId).ToList();
                    }
                    if (request.PriceFrom >= 0 && request.PriceTo != 0)
                    {
                        productList = productList.Where(x => x.Price >= request.PriceFrom && x.Price <= request.PriceTo).ToList();
                    }
                    if (request.SortType == "PriceAsc")
                    {
                        productList = productList.OrderBy(x => x.Price).ToList();
                    }
                    if (request.SortType == "PriceDesc")
                    {
                        productList = productList.OrderByDescending(x => x.Price).ToList();
                    }
                    if (request.SortType == "DateAsc")
                    {
                        productList = productList.OrderBy(x => x.Id).ToList();
                    }
                    if (request.SortType == "DateDesc")
                    {
                        productList = productList.OrderByDescending(x => x.Id).ToList();
                    }
                    productDtos = _mapper.Map<List<UpdateProductDto>>(productList).Paginate(request).ToList();
                    response = new ProductResponse
                    {
                        Product = productDtos,
                        Page = Paging.GetPagingResponse(request, productList.Count())
                    };
                }
                else
                {
                    var query = from p in _context.Product
                                join od in _context.OrderDetail on p.Id equals od.ProductId
                                join o in _context.Order on od.OrderId equals o.Id
                                group new { p, od } by new
                                {
                                    p.Id,
                                    p.Name,
                                    p.Title,
                                    p.Description,
                                    p.Star,
                                    p.Price,
                                    p.Quantity,
                                    p.CategoryId,
                                    p.Thumbnail
                                } into g
                                orderby g.Count() descending
                                select new ProductOrderBy
                                {
                                    Id = g.Key.Id,
                                    Name = g.Key.Name,
                                    Title = g.Key.Title,
                                    Description = g.Key.Description,
                                    Star = g.Key.Star,
                                    Price = g.Key.Price,
                                    Quantity = g.Key.Quantity,
                                    CategoryId = g.Key.CategoryId,
                                    Thumbnail = g.Key.Thumbnail,
                                    QuantitySold = g.Count()
                                };



                    List<ProductOrderBy> productList = await query.ToListAsync();

                    foreach (var item in productList)
                    {
                        var category = _context.Category
                                .Where(c => c.Id == item.CategoryId)
                                .FirstOrDefault();
                        item.Category = category;
                    }
                    productDtos = _mapper.Map<List<UpdateProductDto>>(productList).Paginate(request).ToList();
                    response = new ProductResponse
                    {
                        Product = productDtos,
                        Page = Paging.GetPagingResponse(request, productList.Count())
                    };
                }			

				return response;

			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

		}

 

        public Product GetProductById(int id)
		{
			try
			{
				var query = _context.Product
									.Include(x => x.Category)
									.FirstOrDefault(x => x.Id == id);

				return query;
			}
			catch (Exception ex)
			{
				throw new Exception($"Error getting product by ID: {ex.Message}");
			}
		}

		public List<Product> GetProductByCategoryId(int id)
		{
			try
			{
				var product = GetProductById(id);
				var query = _context.Product
									.Include(x => x.Category)
									.Where(x => x.CategoryId == product.CategoryId)
									.ToList();

				return query;
			}
			catch (Exception ex)
			{
				throw new Exception($"Error getting product by CID: {ex.Message}");
			}
		}


	}
}
