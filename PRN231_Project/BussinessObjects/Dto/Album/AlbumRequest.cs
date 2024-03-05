using BussinessObjects.Paging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Dto.Album
{
    public class AlbumRequest : PagingRequestBase
    {
        public int? Id { get; set; }     
        public int? ProductId { get; set; }    
        public string? Image { get; set; }
    }
}
