using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Dto.Album
{
    public class CreateAlbumDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string Image { get; set; }
    }
}
