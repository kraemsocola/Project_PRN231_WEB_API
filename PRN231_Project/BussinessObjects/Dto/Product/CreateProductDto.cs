using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Dto.Product
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Star { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public string Thumbnail { get; set; }     
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
