using BussinessObjects.Dto;
using BussinessObjects.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.AccountRepo
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> SignUpAsync(SignUpDto model);
        public Task<string> SignInAsync(SignInDto model);
  
        public AppUser GetUserFromDatabase(string token);
    }
}
