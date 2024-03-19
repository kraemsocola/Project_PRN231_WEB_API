using AutoMapper;
using BussinessObjects.Dto.Album;
using BussinessObjects.Helper;
using BussinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.AlbumRepo
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly ProjectDbContext _context;
        private readonly IMapper _mapper;

        public AlbumRepository(ProjectDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AlbumResponse> GetAlbum(AlbumRequest request)
        {
            try
            {   
                var albumQuery = await _context.Album.Include(x => x.Product)
                    .Where(x=> string.IsNullOrWhiteSpace(request.KeyWords) 
                     || x.Image.Contains(request.KeyWords)).ToListAsync();

                if(request.Id != null)
                {
                    albumQuery = albumQuery.Where(x=>x.Id == request.Id).ToList();
                }
                if (request.ProductId != null)
                {
                    albumQuery = albumQuery.Where(x => x.ProductId == request.ProductId).ToList();
                }
                if (request.Image != null)
                {
                    albumQuery = albumQuery.Where(x => x.Image.Contains(request.Image)).ToList();
                }

                var albumDtos = _mapper.Map<List<UpdateAlbumDto>>(albumQuery).Paginate(request).ToList();


                var response = new AlbumResponse
                {
                    Album = albumDtos,
                    Page = Paging.GetPagingResponse(request, albumQuery.Count())
                };

                return response;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

    }
}
