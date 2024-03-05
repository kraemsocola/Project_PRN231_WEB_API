using BussinessObjects.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Dto.Album
{
    public class AlbumResponse : PagingResponsse
    {
        public List<UpdateAlbumDto> Album { get; set; }
    }
}
