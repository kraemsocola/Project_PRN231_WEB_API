using BussinessObjects.Dto.Album;
using BussinessObjects.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Dto.Category
{
    public class CategoryResponse : PagingResponsse
    {
        public List<UpdateCategoryDto> Category { get; set; }
    }
}
