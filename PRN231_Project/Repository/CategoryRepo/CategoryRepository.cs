using AutoMapper;
using BussinessObjects.Dto.Album;
using BussinessObjects.Dto.Category;
using BussinessObjects.Helper;
using BussinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.CategoryRepo
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ProjectDbContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(ProjectDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CategoryResponse> GetCategory(CategoryRequest request)
        {
            try
            {
                var query = await _context.Category
                    .Where(x => string.IsNullOrWhiteSpace(request.KeyWords)
                     || x.Name.Contains(request.KeyWords)).ToListAsync();

                if (request.Id != null)
                {
                    query = query.Where(x => x.Id == request.Id).ToList();
                }
                if (request.Name != null)
                {
                    query = query.Where(x => x.Name.Contains(request.Name)).ToList();
                }

                var dtos = _mapper.Map<List<UpdateCategoryDto>>(query).Paginate(request).ToList();


                var response = new CategoryResponse
                {
                    Category = dtos,
                    Page = Paging.GetPagingResponse(request, query.Count())
                };

                return response;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
