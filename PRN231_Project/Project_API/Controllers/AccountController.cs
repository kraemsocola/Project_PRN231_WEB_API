using BussinessObjects.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.AccountRepo;


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
        public async Task<IActionResult> GetUser(string email)
        {
            // Giải mã token để lấy ID người dùng
            var user = _repo.GetUserFromDatabase(email);

            // Truy vấn cơ sở dữ liệu để lấy thông tin người dùng từ ID

            if (user == null)
            {
                return NotFound(); // Trả về HTTP 404 Not Found nếu không tìm thấy người dùng
            }

            return Ok(user); // Trả về thông tin người dùng
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
                    return Ok(new
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
