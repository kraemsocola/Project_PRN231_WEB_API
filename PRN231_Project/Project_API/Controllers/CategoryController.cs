using AutoMapper;
using BussinessObjects.Dto;
using BussinessObjects.Dto.Album;
using BussinessObjects.Dto.Category;
using BussinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.AlbumRepo;
using Repository.CategoryRepo;

namespace Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ProjectDbContext _context;
        private ICategoryRepository _repository;
        private ResponseDto _response;
        private readonly IMapper _mapper;
        public CategoryController(ProjectDbContext context, IMapper mapper, ICategoryRepository categoryRepository)
        {
            _context = context;
            _mapper = mapper;
            _response = new ResponseDto();
            _repository = categoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<CategoryResponse>> GetCategory([FromQuery] CategoryRequest request)
        {
            var data = await _repository.GetCategory(request);
            return Ok(data);
        }

        [HttpPut]
        public ResponseDto Put([FromBody] UpdateCategoryDto cate)
        {
            try
            {
                Category obj = _mapper.Map<Category>(cate);
                _context.Category.Update(obj);
                _context.SaveChanges();
                _response.Result = _mapper.Map<UpdateCategoryDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        public ResponseDto Post([FromBody] CreateCategoryDto cateDto)
        {
            try
            {
                Category obj = _mapper.Map<Category>(cateDto);
                _context.Category.Add(obj);
                _context.SaveChanges();
                _response.Result = _mapper.Map<CreateCategoryDto>(obj);
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
                var cate = _context.Category.FirstOrDefault(x => x.Id == id);

                // xóa Product trước
                var product = _context.Product.Where(x => x.Id == id).FirstOrDefault();
                if (product != null)
                {
                    _context.Product.Remove(product);
                    _context.SaveChanges();
                }

                _context.Category.Remove(cate);
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
