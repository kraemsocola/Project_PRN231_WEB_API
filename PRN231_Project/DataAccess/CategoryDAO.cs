using AutoMapper;
using BussinessObjects.Dto;
using BussinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataAccess
{
    public class CategoryDAO
    {
        private static readonly IMapper _mapper;
        public static List<Category> GetAllList()
        {
            var list = new List<Category>();
            try
            {
                using (var context = new ProjectDbContext())
                {
                    list = context.Category.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }

        public static List<Category> SearchByName(string name)
        {
            var list = new List<Category>();
            try
            {
                using (var context = new ProjectDbContext())
                {
                    list = context.Category.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }

        public static Category FindCategoryById(int id)
        {
            Category s = new Category();
            try
            {
                using (var context = new ProjectDbContext())
                {
                    s = context.Category.Find(id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return s;
        }

        public static void Delete(Category s)
        {
            try
            {
                using (var context = new ProjectDbContext())
                {                
                    context.Category.Remove(s);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public static void Add(CategoryDto model)
        {
            try
            {
                using (var context = new ProjectDbContext())
                {
                    var obj = new Category
                    {
                        Name = model.Name
                    };

                    /*var obj = _mapper.Map<Category>(model);*/
                    context.Category.Add(obj);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void Update(Category model)
        {
            try
            {
                using (var context = new ProjectDbContext())
                {
                    var s1 = context.Category.Find(model.Id);
                    if (s1 != null)
                    {
                        s1.Name = model.Name;
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
