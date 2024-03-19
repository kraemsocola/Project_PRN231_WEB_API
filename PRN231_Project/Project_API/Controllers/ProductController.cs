using AutoMapper;
using BussinessObjects.Dto;
using BussinessObjects.Dto.Album;
using BussinessObjects.Dto.OrderDetail;
using BussinessObjects.Dto.Product;
using BussinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.AlbumRepo;
using Repository.ProductRepo;

namespace Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly ProjectDbContext _context;
        private ResponseDto _response;
        private readonly IMapper _mapper;
		private IProductRepository _repository;
		public ProductController(ProjectDbContext context, IMapper mapper, IProductRepository productRepository)
        {
            _context = context;
            _mapper = mapper;
            _response = new ResponseDto();
            _repository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ProductResponse>> GetProduct([FromQuery]ProductRequest request)
        {
            var data = await _repository.GetProduct(request);
            return Ok(data);
        }

		[HttpPut]
        public ResponseDto Put([FromBody] UpdateProductDto productDto)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDto);
                _context.Product.Update(product);
                _context.SaveChanges();
                _response.Result = _mapper.Map<UpdateProductDto>(product);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        public ResponseDto Post([FromBody] CreateProductDto productDto)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDto);
                _context.Product.Add(product);
                _context.SaveChanges();
                _response.Result = _mapper.Map<CreateProductDto>(product);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpDelete("{id}")]
        public ResponseDto Delete(int id)
        {
            try
            {
                var product = _context.Product.FirstOrDefault(x => x.Id == id);

                // xóa Feedback trước
                var feedback = _context.Feedback.Where(x => x.Id == id).FirstOrDefault();
                if (feedback != null)
                {
                    _context.Feedback.Remove(feedback);
                    _context.SaveChanges();
                }
                // xóa OrderDetail trước
                var orderdetail = _context.OrderDetail.Where(x => x.Id == id).FirstOrDefault();
                if (orderdetail != null)
                {
                    _context.OrderDetail.Remove(orderdetail);
                    _context.SaveChanges();
                }
                // xóa Album trước
                var album = _context.Album.Where(x => x.Id == id).FirstOrDefault();
                if (album != null)
                {
                    _context.Album.Remove(album);
                    _context.SaveChanges();
                }

                _context.Product.Remove(product);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;

        }
    }
}
