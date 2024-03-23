using BussinessObjects.Dto.LogInLogOut;
using BussinessObjects.Dto.User;
using BussinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.AccountRepo;
using System.IdentityModel.Tokens.Jwt;

namespace Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _repo;
        

        public AccountController(IAccountRepository repo)
        {
            _repo = repo;
          
        }    

        [HttpGet("GetUser")]
        public IActionResult GetUser(string token)
        {
            // Lấy token từ session
            

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(); // Trả về lỗi 401 nếu không có token trong session
            }

            // Giải mã token để lấy các thông tin trong token
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            // Lấy thông tin người dùng từ token
            var email = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            var phoneNumber = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone")?.Value;
            var streetAddress = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/streetaddress")?.Value;
            var role = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

            // Tại đây bạn có thể sử dụng userId hoặc userEmail để truy vấn thông tin của người dùng từ cơ sở dữ liệu hoặc từ nguồn dữ liệu khác
            // Ví dụ: var user = _userService.GetUserById(userId);

            // Trả về thông tin người dùng (ví dụ: id và email) trong response của API
            return Ok(new UserResponse
            {
                Email = email,
                PhoneNumber = phoneNumber,
                Address = streetAddress,
                Role = role
            });
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpDto model)
        {
            if(model.Password != model.ConfirmPassword)
            {
                return BadRequest("Password and ConfirmPass is not match");
            }
            else
            {
                var result = await _repo.SignUpAsync(model);
                if (result.Succeeded)
                {
                    return Ok(new SignUpResponse
                    {
                        Success = true,
                        Message = "User registered successfully. "
                    });
                }
                // Trả về HTTP Status Code 400 Bad Request và một đối tượng JSON với danh sách lỗi
                return BadRequest(new { Success = false, Errors = result.Errors.Select(e => e.Description) });
            }
        }



        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInDto model)
        {
            var result = await _repo.SignInAsync(model);
            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized(new { Success = false, Message = "Invalid username or password."});
            }
            
            return Ok(new SignInResponse
            {
                Success = true,
                Token = result,
                Message = "Login successful."
            });
        }
    }
}
