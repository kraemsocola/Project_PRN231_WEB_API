using AutoMapper;
using BussinessObjects.Dto;
using BussinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Repositories.CategoryRepo;

namespace Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _repo;
        private ResponseDto _response;
        public CategoryController(ICategoryRepository repo)
        {
            _repo = repo;
            _response = new ResponseDto();

        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                _response.Result = _repo.GetAll();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;

        }

        [HttpPut]
        public ResponseDto Put([FromBody] Category category)
        {
            try
            {
                var check = _repo.GetById(category.Id);
                if (check == null)
                {
                    _response.IsSuccess = false;
                    return _response;
                }
                _repo.Update(category);
                _response.Result = category;
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
                _repo.Create(cateDto);
                _response.Result = cateDto;
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
                var check = _repo.GetById(id);
                if (check == null)
                {
                    _response.IsSuccess = false;
                    return _response;
                }
                _repo.Delete(check);
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
