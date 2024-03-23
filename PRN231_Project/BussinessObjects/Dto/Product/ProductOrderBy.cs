using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Dto.Product
{
    public class ProductOrderBy
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Star { get; set; }

       
        public int Price { get; set; }

      
        public string Thumbnail { get; set; }

     
        public int Quantity { get; set; }

      
        public int CategoryId { get; set; }

        
        public Models.Category Category { get; set; }

        // Thêm thuộc tính mới để lưu trữ tổng số lượng đã bán
        public int QuantitySold { get; set; }
    }
}
