using AutoMapper;
using BussinessObjects.Dto;
using BussinessObjects.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CategoryRepo
{
    public class CategoryRepository : ICategoryRepository
    {       

        public void Create(CategoryDto model)
        {
           CategoryDAO.Add(model);
        }

        public void Delete(Category s)
        {
            CategoryDAO.Delete(s);
        }

        public List<Category> GetAll()
        {
            return CategoryDAO.GetAllList();
        }

        public Category GetById(int id)
        {
            return CategoryDAO.FindCategoryById(id);
        }

        public List<Category> SearchByName(string name)
        {
            return CategoryDAO.SearchByName(name);
        }

        public void Update(Category model)
        {
            CategoryDAO.Update(model);
        }
    }
}
