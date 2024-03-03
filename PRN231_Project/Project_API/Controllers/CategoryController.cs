using AutoMapper;
using BussinessObjects.Dto;
using BussinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ProjectDbContext _context;
        private ResponseDto _response;
        private readonly IMapper _mapper;
        public CategoryController(ProjectDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet("search/{text}")]
        public ResponseDto SearchCategory(string text)
        {
            try
            {
                List<Category> categories = _context.Category.Where(x=>x.Name.Contains(text)).ToList(); 

                _response.Result = categories;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;

        }

        [HttpGet("{id}")]
        public ResponseDto GetCategoryById(int id)
        {
            try
            {
                Category cate = _context.Category.Where(x => x.Id == id).First();

                _response.Result = cate;
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
                List<Category> cate = _context.Category.ToList();

                _response.Result = cate;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;

        }

        [HttpPut]
        public ResponseDto Put([FromBody] Category cate)
        {
            try
            {             
                _context.Category.Update(cate);
                _context.SaveChanges();
                _response.Result = cate;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        public ResponseDto Post([FromBody] CategoryDto cateDto)
        {
            try
            {
                Category obj = _mapper.Map<Category>(cateDto);
                _context.Category.Add(obj);
                _context.SaveChanges();
                _response.Result = _mapper.Map<CategoryDto>(obj);
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
