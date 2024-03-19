using BussinessObjects.Dto.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.CategoryRepo
{
    public interface ICategoryRepository
    {
        Task<CategoryResponse> GetCategory(CategoryRequest request);
    }
}
