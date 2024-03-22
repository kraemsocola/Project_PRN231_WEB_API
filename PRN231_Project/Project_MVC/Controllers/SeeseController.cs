using BussinessObjects.Dto;
using BussinessObjects.Dto.Category;
using BussinessObjects.Dto.Product;
using BussinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.AccountRepo;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Project_MVC.Controllers
{
    public class SeeseController : Controller
    {
        private readonly ILogger<SeeseController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient client = null;
        private string UrlApi = "";


        public SeeseController(ILogger<SeeseController> logger, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            UrlApi = "https://localhost:7019/api/";
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Home()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(string email, string password)
        {
            try
            {
                // Tạo đối tượng SignInDto từ email và password
                var signInDto = new SignInDto { Email = email, Password = password };

                // Chuyển đổi SignInDto thành nội dung JSON
                var jsonContent = System.Text.Json.JsonSerializer.Serialize(signInDto);

                // Tạo nội dung HTTP từ JSON
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Gửi yêu cầu POST đến API SignIn
                var response = await client.PostAsync(UrlApi + "Account/SignIn", httpContent);

                // Đảm bảo yêu cầu được thực hiện thành công
                response.EnsureSuccessStatusCode();

                // Đọc nội dung phản hồi
                var responseBody = await response.Content.ReadAsStringAsync();

                // Chuyển đổi nội dung JSON thành đối tượng
                SignInResponse signInResponse = JsonConvert.DeserializeObject<SignInResponse>(responseBody);

                if (signInResponse.Success)
                {
                    // Đăng nhập thành công, lưu token vào session và chuyển hướng đến trang chính
                    _httpContextAccessor.HttpContext.Session.SetString("Token", signInResponse.Token);
                    var getUser = await client.GetAsync(UrlApi + "Account/GetUser?email=" + email);
                    if (getUser.IsSuccessStatusCode)
                    {
                        string content = await getUser.Content.ReadAsStringAsync();
                        var option = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                        };
                        AppUser user = System.Text.Json.JsonSerializer.Deserialize<AppUser>(content, option);
                        ViewBag.User = user;
                    }
                    return View("Home");
                }
                else
                {
                    // Đăng nhập không thành công, hiển thị thông báo lỗi
                    ViewBag.ErrorMessage = signInResponse.Message;
                    return View("Login"); // Trả về view đăng nhập
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                ViewBag.ErrorMessage = "An error occurred while processing your request: " + ex.Message;
                return View("Login"); // Trả về view đăng nhập
            }
        }


        [HttpGet]
        public async Task<IActionResult> Filter(int min_price, int max_price, int category_id)
        {

            int itemPerPage;

            try
            {
                itemPerPage = 12;
                HttpResponseMessage getAllProduct;

                getAllProduct = await client.GetAsync(UrlApi + "Product?PriceFrom=" + min_price + "&PriceTo=" + max_price + "&ItemsPerPage=" + itemPerPage + "&CategoryId=" + category_id);

                if (getAllProduct.IsSuccessStatusCode)
                {
                    string content = await getAllProduct.Content.ReadAsStringAsync();
                    var option = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    ProductResponse product = System.Text.Json.JsonSerializer.Deserialize<ProductResponse>(content, option);
                    ViewBag.CategoryId = 0;
                    ViewBag.Min_Price = min_price;
                    ViewBag.Max_Price = max_price;
                    ViewBag.Products = product.Product;
                    ViewBag.Pages = product.Page;
                }

                itemPerPage = Int32.MaxValue;
                HttpResponseMessage getAllCategory = await client.GetAsync(UrlApi + "Category?ItemsPerPage=" + itemPerPage);
                if (getAllCategory.IsSuccessStatusCode)
                {
                    string content = await getAllCategory.Content.ReadAsStringAsync();
                    var option = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    CategoryResponse category = System.Text.Json.JsonSerializer.Deserialize<CategoryResponse>(content, option);
                    ViewBag.Categories = category.Category;

                }

                return View("Shop");
            }
            catch (HttpRequestException ex)
            {
                // Xử lý các lỗi liên quan đến yêu cầu HTTP
                return BadRequest(ex.Message);
            }
        }



        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                HttpResponseMessage responseMessage1 = await client.GetAsync(UrlApi + "Product/" + id);
                if (responseMessage1.IsSuccessStatusCode)
                {
                    string content = await responseMessage1.Content.ReadAsStringAsync();
                    var option = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    UpdateProductDto product = System.Text.Json.JsonSerializer.Deserialize<UpdateProductDto>(content, option);
                    ViewBag.UpdateProductDto = product;
                }

                HttpResponseMessage responseMessage2 = await client.GetAsync(UrlApi + "Product/Category/" + id);
                if (responseMessage2.IsSuccessStatusCode)
                {
                    string content = await responseMessage2.Content.ReadAsStringAsync();
                    var option = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    List<UpdateProductDto> product = System.Text.Json.JsonSerializer.Deserialize<List<UpdateProductDto>>(content, option);
                    ViewBag.ListUpdateProductDto = product;
                }

                return View();
            }
            catch (HttpRequestException ex)
            {
                // Xử lý các lỗi liên quan đến yêu cầu HTTP
                return BadRequest(ex.Message);
            }

        }

        public async Task<IActionResult> Shop(int? pageNumber, int? categoryId, int? priceFrom, int? priceTo)
        {
            int itemPerPage;
            //Lần đầu vào shop thì các tham số đều là null
            if (priceFrom == null && priceTo == null)
            {
                priceFrom = 10;
                priceTo = 200;
            }
            if (categoryId == null)
            {
                categoryId = 0;
            }
            if (pageNumber == null)
            {
                pageNumber = 1;
            }

            try
            {
                itemPerPage = 12;
                HttpResponseMessage getAllProduct;
                if (categoryId == 0)
                {
                    getAllProduct = await client.GetAsync(UrlApi + "Product?Page=" + pageNumber + "&PriceFrom=" + priceFrom + "&PriceTo=" + priceTo + "&ItemsPerPage=" + itemPerPage);
                }
                else
                {
                    getAllProduct = await client.GetAsync(UrlApi + "Product?CategoryId=" + categoryId + "&Page=" + pageNumber + "&PriceFrom=" + priceFrom + "&PriceTo=" + priceTo + "&ItemsPerPage=" + itemPerPage);
                }
                if (getAllProduct.IsSuccessStatusCode)
                {
                    string content = await getAllProduct.Content.ReadAsStringAsync();
                    var option = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    ProductResponse product = System.Text.Json.JsonSerializer.Deserialize<ProductResponse>(content, option);
                    ViewBag.Min_Price = priceFrom;
                    ViewBag.Max_Price = priceTo;
                    ViewBag.CategoryId = categoryId;
                    ViewBag.Products = product.Product;
                    ViewBag.Pages = product.Page;
                }

                itemPerPage = Int32.MaxValue;
                HttpResponseMessage getAllCategory = await client.GetAsync(UrlApi + "Category?ItemsPerPage=" + itemPerPage);
                if (getAllCategory.IsSuccessStatusCode)
                {
                    string content = await getAllCategory.Content.ReadAsStringAsync();
                    var option = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    CategoryResponse category = System.Text.Json.JsonSerializer.Deserialize<CategoryResponse>(content, option);
                    ViewBag.Categories = category.Category;

                }

                return View();
            }
            catch (HttpRequestException ex)
            {
                // Xử lý các lỗi liên quan đến yêu cầu HTTP
                return BadRequest(ex.Message);
            }
        }


    }
}
