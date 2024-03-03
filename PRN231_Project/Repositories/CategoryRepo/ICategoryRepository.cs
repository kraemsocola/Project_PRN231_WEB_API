using BussinessObjects.Dto;
using BussinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CategoryRepo
{
    public interface ICategoryRepository
    {
        public List<Category> GetAll();
        public List<Category> SearchByName(string name);
        public Category GetById(int id);
        public void Delete(Category s);
        public void Create(CategoryDto model);
        public void Update(Category model);
    }
}
