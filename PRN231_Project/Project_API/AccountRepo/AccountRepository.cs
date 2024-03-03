using BussinessObjects.Dto;
using BussinessObjects.Helper;
using BussinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Project_API.AccountRepo
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AccountRepository(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager)
        {
           _userManager = userManager;
           _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> SignUpAsync(SignUpDto model)
        {
            var isExitst = await _userManager.FindByEmailAsync(model.Email);
            if (isExitst != null)
            {
                return IdentityResult.Failed(new IdentityError { Code = "EmailAlreadyTaken", Description = "Email is already taken." });
            }

            var newUser = new AppUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Address = model.Address,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);
            // sau khi tạo được user,thì addRole mặc định cho user mới này
            if (!result.Succeeded)
            {
                return IdentityResult.Failed(new IdentityError { Code = "RegisterFailed", Description = "User creation failed! Please check user details and try again." });

            }
            else
            {
                // check Role mặc định đã tồn tại trong DB chưa
                if(!await _roleManager.RoleExistsAsync(AppRole.Customer))
                {
                    await _roleManager.CreateAsync(new IdentityRole(AppRole.Customer));
                }
                // đăng kí role mặc định đó cho user
                await _userManager.AddToRoleAsync(newUser, AppRole.Customer);    
            }

            return result;
        }





        public async Task<string> SignInAsync(SignInDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (user == null || !passwordValid)
            {
                return string.Empty;
            }
            // định nghĩa ra các claim cho user
            var authClaims = new List<Claim> {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.MobilePhone,user.PhoneNumber),
                new Claim(ClaimTypes.StreetAddress, user.Address),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }
            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(20),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
