using AutoMapper;
using BussinessObjects.Dto;
using BussinessObjects.Dto.OrderDetail;
using BussinessObjects.Models;
using Microsoft.AspNetCore.Mvc;

namespace Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : Controller
    {
        private readonly ProjectDbContext _context;
        private ResponseDto _response;
        private readonly IMapper _mapper;
        public OrderDetailController(ProjectDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet("Order/{id}")]
        public ResponseDto GetOrderDetailByOrder(int id)
        {
            try
            {
                List<OrderDetail> orderDetails = _context.OrderDetail.Where(x => x.OrderId == id).ToList();

                _response.Result = _mapper.Map<List<UpdateOrderDetailDto>>(orderDetails);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;

        }

        [HttpGet("Product/{id}")]
        public ResponseDto GetOrderDetailByProduct(int id)
        {
            try
            {
                List<OrderDetail> orderDetails = _context.OrderDetail.Where(x => x.ProductId == id).ToList();

                _response.Result = _mapper.Map<List<UpdateOrderDetailDto>>(orderDetails);
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
                List<OrderDetail> orderDetails = _context.OrderDetail.ToList();

                _response.Result = _mapper.Map<List<UpdateOrderDetailDto>>(orderDetails);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;

        }

        [HttpPut]
        public ResponseDto Put([FromBody] UpdateOrderDetailDto orderDetailDto)
        {
            try
            {
                OrderDetail orderDetail = _mapper.Map<OrderDetail>(orderDetailDto);
                _context.OrderDetail.Update(orderDetail);
                _context.SaveChanges();
                _response.Result = _mapper.Map<UpdateOrderDetailDto>(orderDetail);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        public ResponseDto Post([FromBody] CreateOrderDetailDto orderDetailDto)
        {
            try
            {
                OrderDetail orderDetail = _mapper.Map<OrderDetail>(orderDetailDto);
                _context.OrderDetail.Add(orderDetail);
                _context.SaveChanges();
                _response.Result = _mapper.Map<CreateOrderDetailDto>(orderDetail);
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
                var orderDetail = _context.OrderDetail.FirstOrDefault(x => x.Id == id);
                _context.OrderDetail.Remove(orderDetail);
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
