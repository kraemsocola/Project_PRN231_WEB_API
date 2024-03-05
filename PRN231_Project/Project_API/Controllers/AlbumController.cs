using AutoMapper;
using BussinessObjects.Dto;
using BussinessObjects.Dto.Album;
using BussinessObjects.Dto.Category;
using BussinessObjects.Dto.Product;
using BussinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.AlbumRepo;
using static System.Reflection.Metadata.BlobBuilder;

namespace Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : Controller
    {      
        private ResponseDto _response;
        private IAlbumRepository _repository;
        private readonly ProjectDbContext _context;
        private readonly IMapper _mapper;
        public AlbumController(IAlbumRepository albumRepository, IMapper mapper, ProjectDbContext context)
        {
            _response = new ResponseDto();
            _repository = albumRepository;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<AlbumResponse>> GetAlbum([FromQuery] AlbumRequest request)
        {
            var data = await _repository.GetAlbum(request);
            return Ok(data);
        }

        [HttpPut]
        public ResponseDto Put([FromBody] UpdateAlbumDto data)
        {
            try
            {
                Album obj = _mapper.Map<Album>(data);
                _context.Album.Update(obj);
                _context.SaveChanges();
                _response.Result = _mapper.Map<UpdateAlbumDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        public ResponseDto CreateAlbum([FromBody] CreateAlbumDto data)
        {           
            try
            {
                Album obj = _mapper.Map<Album>(data);
                _context.Album.Add(obj);
                _context.SaveChanges();
                _response.Result = _mapper.Map<CreateAlbumDto>(obj);
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
                var obj = _context.Album.FirstOrDefault(x => x.Id == id);
                _context.Album.Remove(obj);
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
