using BussinessObjects.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Dto.OrderDetail
{
    public class CreateOrderDetailDto
    {

        [Required]
        public int OrderId { get; set; }
        [Required]
        public int ProductId { get; set; }

    }
}
