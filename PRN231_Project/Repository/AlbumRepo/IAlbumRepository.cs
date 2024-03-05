using BussinessObjects.Dto.Album;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.AlbumRepo
{
    public interface IAlbumRepository
    {
        Task<AlbumResponse> GetAlbum(AlbumRequest request);
        
    }
}
