using AutoMapper;
using BussinessObjects.Dto;
using BussinessObjects.Dto.Product;
using BussinessObjects.Models;
using Microsoft.AspNetCore.Mvc;

namespace Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly ProjectDbContext _context;
        private ResponseDto _response;
        private readonly IMapper _mapper;
        public ProductController(ProjectDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet("search/{text}")]
        public ResponseDto SearchProduct(string text)
        {
            try
            {
                List<Product> products = _context.Product.Where(x => x.Name.Contains(text)).ToList();

                _response.Result = _mapper.Map<List<UpdateProductDto>>(products);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;

        }

        [HttpGet("{id}")]
        public ResponseDto GetProductById(int id)
        {
            try
            {
                Product product = _context.Product.Where(x => x.Id == id).First();

                _response.Result = _mapper.Map<UpdateProductDto>(product);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;

        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                List<Product> products = _context.Product.ToList();

                _response.Result = _mapper.Map<List<UpdateProductDto>>(products);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;

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
