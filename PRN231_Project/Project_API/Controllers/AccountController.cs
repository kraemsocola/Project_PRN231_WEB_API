using BussinessObjects.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.AccountRepo;

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
            return Ok(new
            {
                Success = true,
                Token = result,
                Message = "Login successful."
            });
        }
    }
}
