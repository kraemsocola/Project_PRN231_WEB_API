using BussinessObjects.Dto.Category;
using BussinessObjects.Dto.LogInLogOut;
using BussinessObjects.Dto.Product;
using BussinessObjects.Dto.User;
using BussinessObjects.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.AccountRepo;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BussinessObjects.Dto;

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

        public async Task<IActionResult> Home()
        {
            List<CartDto> shoppingCart;
            if (_httpContextAccessor.HttpContext.Session.TryGetValue("ShoppingCart", out var cartData))
            {
                shoppingCart = JsonConvert.DeserializeObject<List<CartDto>>(Encoding.UTF8.GetString(cartData));
                //Kiểm tra xem có dữ liệu giỏ hàng đã được lưu trong Session không. Phương thức TryGetValue của
                //Session được sử dụng để thử lấy dữ liệu từ khóa "ShoppingCart". Nếu dữ liệu tồn tại, biến cartData
                //sẽ chứa dữ liệu của giỏ hàng.
            }
            else
            {
                shoppingCart = new List<CartDto>();
            }
            ViewBag.Carts = shoppingCart;

            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var getUser = await client.GetAsync(UrlApi + "Account/GetUser?Token=" + token);
            if (getUser.IsSuccessStatusCode)
            {
                string content = await getUser.Content.ReadAsStringAsync();
                var option = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                UserResponse user = System.Text.Json.JsonSerializer.Deserialize<UserResponse>(content, option);
                ViewBag.User = user;
            }

            return View();
        }       

        // Thêm một mục vào giỏ hàng
        public async Task<IActionResult> AddToCart(CartDto item)
        {
            bool alert=false;
            List<CartDto> shoppingCart;
            try
            {
                if (_httpContextAccessor.HttpContext.Session.TryGetValue("ShoppingCart", out var cartData))
                {
                    shoppingCart = JsonConvert.DeserializeObject<List<CartDto>>(Encoding.UTF8.GetString(cartData));
                    //Kiểm tra xem có dữ liệu giỏ hàng đã được lưu trong Session không. Phương thức TryGetValue của
                    //Session được sử dụng để thử lấy dữ liệu từ khóa "ShoppingCart". Nếu dữ liệu tồn tại, biến cartData
                    //sẽ chứa dữ liệu của giỏ hàng.
                }
                else
                {
                    shoppingCart = new List<CartDto>();
                }

                shoppingCart.Add(item);
                alert= true;

                _httpContextAccessor.HttpContext.Session.Set("ShoppingCart", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(shoppingCart)));
                ViewBag.Carts = shoppingCart;
               
                var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
                var getUser = await client.GetAsync(UrlApi + "Account/GetUser?Token=" + token);
                if (getUser.IsSuccessStatusCode)
                {
                    string content = await getUser.Content.ReadAsStringAsync();
                    var option = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    UserResponse user = System.Text.Json.JsonSerializer.Deserialize<UserResponse>(content, option);
                    ViewBag.User = user;
                }
                TempData["Alert"] = alert;
                return RedirectToAction("Detail", new { id = item.ProductId });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                ViewBag.Message = "An error occurred while processing your request: " + ex.Message;
                return View("Detail"); // Trả về view đăng nhập
            }
        }

        public IActionResult RemoveFromCart(int productId)
        {
            try
            {
                List<CartDto> shoppingCart;
                if (_httpContextAccessor.HttpContext.Session.TryGetValue("ShoppingCart", out var cartData))
                {
                    shoppingCart = JsonConvert.DeserializeObject<List<CartDto>>(Encoding.UTF8.GetString(cartData));
                    var itemToRemove = shoppingCart.FirstOrDefault(item => item.ProductId == productId);
                    if (itemToRemove != null)
                    {
                        shoppingCart.Remove(itemToRemove);
                    }
                }
                else
                {
                    shoppingCart = new List<CartDto>();
                }

                _httpContextAccessor.HttpContext.Session.Set("ShoppingCart", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(shoppingCart)));

                return RedirectToAction("Cart", "Seese");   
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu cần
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost]
        public IActionResult UpdateCartItem(Dictionary<string, int> cart)
        {
            try
            {
                List<CartDto> shoppingCart;
                if (_httpContextAccessor.HttpContext.Session.TryGetValue("ShoppingCart", out var cartData))
                {
                    shoppingCart = JsonConvert.DeserializeObject<List<CartDto>>(Encoding.UTF8.GetString(cartData));
                    foreach (var item in shoppingCart)
                    {
                        if (cart.ContainsKey(item.ProductId.ToString())) // Check if the product is in the updated cart
                        {
                            item.Quantity = cart[item.ProductId.ToString()]; // Update the quantity of the product
                        }
                    }
                }
                else
                {
                    shoppingCart = new List<CartDto>();
                }

                _httpContextAccessor.HttpContext.Session.Set("ShoppingCart", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(shoppingCart)));

                // Optionally, you can return a JSON response to indicate the success of the update
                return RedirectToAction("Cart", "Seese");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu cần
                return StatusCode(500, ex.Message);
            }
        }

        // Lấy thông tin giỏ hàng
        public async Task<IActionResult> Cart()
        {
            try
            {
                if (HttpContext.Session.TryGetValue("ShoppingCart", out var cartData))
                {
                    var shoppingCart = JsonConvert.DeserializeObject<List<CartDto>>(Encoding.UTF8.GetString(cartData));
                    ViewBag.Carts = shoppingCart;
                    
                }

                var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
                var getUser = await client.GetAsync(UrlApi + "Account/GetUser?Token=" + token);
                if (getUser.IsSuccessStatusCode)
                {
                    string content = await getUser.Content.ReadAsStringAsync();
                    var option = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    UserResponse user = System.Text.Json.JsonSerializer.Deserialize<UserResponse>(content, option);
                    ViewBag.User = user;
                }
                return View("Cart");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                ViewBag.Message = "An error occurred while processing your request: " + ex.Message;
                return View("Cart"); // Trả về view đăng nhập
            }
        }


        public IActionResult Login()
        {
            List<CartDto> shoppingCart;
            if (_httpContextAccessor.HttpContext.Session.TryGetValue("ShoppingCart", out var cartData))
            {
                shoppingCart = JsonConvert.DeserializeObject<List<CartDto>>(Encoding.UTF8.GetString(cartData));
                //Kiểm tra xem có dữ liệu giỏ hàng đã được lưu trong Session không. Phương thức TryGetValue của
                //Session được sử dụng để thử lấy dữ liệu từ khóa "ShoppingCart". Nếu dữ liệu tồn tại, biến cartData
                //sẽ chứa dữ liệu của giỏ hàng.
            }
            else
            {
                shoppingCart = new List<CartDto>();
            }
            ViewBag.Carts = shoppingCart;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(string firstName, string lastName, string email, string address, string password, string confirmPassword, string phoneNumber)
        {
            try
            {
                // Tạo đối tượng SignInDto từ email và password
                var signUpDto = new SignUpDto { FirstName = firstName, LastName = lastName, Email = email, Address = address, Password = password, ConfirmPassword = confirmPassword, PhoneNumber = phoneNumber };

                // Chuyển đổi SignInDto thành nội dung JSON
                var jsonContent = System.Text.Json.JsonSerializer.Serialize(signUpDto);

                // Tạo nội dung HTTP từ JSON
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Gửi yêu cầu POST đến API SignIn
                var response = await client.PostAsync(UrlApi + "Account/SignUp", httpContent);

                // Đảm bảo yêu cầu được thực hiện thành công
                response.EnsureSuccessStatusCode();

                // Đọc nội dung phản hồi
                var responseBody = await response.Content.ReadAsStringAsync();

                // Chuyển đổi nội dung JSON thành đối tượng
                SignUpResponse signInResponse = JsonConvert.DeserializeObject<SignUpResponse>(responseBody);

                if (signInResponse.Success)
                {
                    ViewBag.Message = "Sign Up Success!!!";
                    return View("Login");
                }
                else
                {
                    // Đăng nhập không thành công, hiển thị thông báo lỗi
                    ViewBag.Message = signInResponse.Message;
                    return View("Login"); // Trả về view đăng nhập
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                ViewBag.Message = "An error occurred while processing your request: " + ex.Message;
                return View("Login"); // Trả về view đăng nhập
            }
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
                    var getUser = await client.GetAsync(UrlApi + "Account/GetUser?Token=" + signInResponse.Token);
                    if (getUser.IsSuccessStatusCode)
                    {
                        string content = await getUser.Content.ReadAsStringAsync();
                        var option = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                        };
                        UserResponse user = System.Text.Json.JsonSerializer.Deserialize<UserResponse>(content, option);
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


        public async Task<IActionResult> LogOut()
        {
            try
            {
                // Xóa token khỏi session khi người dùng đăng xuất
                _httpContextAccessor.HttpContext.Session.Remove("Token");


                ViewBag.User = null;

                // Chuyển hướng đến trang đăng nhập sau khi đăng xuất
                return View("Home");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                ViewBag.ErrorMessage = "An error occurred while processing your request: " + ex.Message;
                return View("Login"); // Hoặc trả về view error nếu muốn
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

                var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
                var getUser = await client.GetAsync(UrlApi + "Account/GetUser?Token=" + token);
                if (getUser.IsSuccessStatusCode)
                {
                    string content = await getUser.Content.ReadAsStringAsync();
                    var option = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    UserResponse user = System.Text.Json.JsonSerializer.Deserialize<UserResponse>(content, option);
                    ViewBag.User = user;
                }

                List<CartDto> shoppingCart;
                if (_httpContextAccessor.HttpContext.Session.TryGetValue("ShoppingCart", out var cartData))
                {
                    shoppingCart = JsonConvert.DeserializeObject<List<CartDto>>(Encoding.UTF8.GetString(cartData));
                    //Kiểm tra xem có dữ liệu giỏ hàng đã được lưu trong Session không. Phương thức TryGetValue của
                    //Session được sử dụng để thử lấy dữ liệu từ khóa "ShoppingCart". Nếu dữ liệu tồn tại, biến cartData
                    //sẽ chứa dữ liệu của giỏ hàng.
                }
                else
                {
                    shoppingCart = new List<CartDto>();
                }
                ViewBag.Carts = shoppingCart;

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

                var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
                var getUser = await client.GetAsync(UrlApi + "Account/GetUser?Token=" + token);
                if (getUser.IsSuccessStatusCode)
                {
                    string content = await getUser.Content.ReadAsStringAsync();
                    var option = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    UserResponse user = System.Text.Json.JsonSerializer.Deserialize<UserResponse>(content, option);
                    ViewBag.User = user;
                }

                List<CartDto> shoppingCart;
                if (_httpContextAccessor.HttpContext.Session.TryGetValue("ShoppingCart", out var cartData))
                {
                    shoppingCart = JsonConvert.DeserializeObject<List<CartDto>>(Encoding.UTF8.GetString(cartData));
                    //Kiểm tra xem có dữ liệu giỏ hàng đã được lưu trong Session không. Phương thức TryGetValue của
                    //Session được sử dụng để thử lấy dữ liệu từ khóa "ShoppingCart". Nếu dữ liệu tồn tại, biến cartData
                    //sẽ chứa dữ liệu của giỏ hàng.
                }
                else
                {
                    shoppingCart = new List<CartDto>();
                }
                ViewBag.Carts = shoppingCart;
                ViewBag.Alert = TempData["Alert"];
                return View();
            }
            catch (HttpRequestException ex)
            {
                // Xử lý các lỗi liên quan đến yêu cầu HTTP
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        public async Task<IActionResult> Search(string? KeyWords)
        {    

            try
            { 
                HttpResponseMessage getAllProduct;               
                
                getAllProduct = await client.GetAsync(UrlApi + "Product?KeyWords=" + KeyWords);
                
                if (getAllProduct.IsSuccessStatusCode)
                {
                    string content = await getAllProduct.Content.ReadAsStringAsync();
                    var option = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    ProductResponse product = System.Text.Json.JsonSerializer.Deserialize<ProductResponse>(content, option);
                    ViewBag.Min_Price = 10;
                    ViewBag.Max_Price = 200;
                    ViewBag.CategoryId = 0;
                    ViewBag.Products = product.Product;
                    ViewBag.Pages = product.Page;
                }

                
                HttpResponseMessage getAllCategory = await client.GetAsync(UrlApi + "Category?ItemsPerPage=" + Int32.MaxValue);
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

                var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
                var getUser = await client.GetAsync(UrlApi + "Account/GetUser?Token=" + token);
                if (getUser.IsSuccessStatusCode)
                {
                    string content = await getUser.Content.ReadAsStringAsync();
                    var option = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    UserResponse user = System.Text.Json.JsonSerializer.Deserialize<UserResponse>(content, option);
                    ViewBag.User = user;
                }

                List<CartDto> shoppingCart;
                if (_httpContextAccessor.HttpContext.Session.TryGetValue("ShoppingCart", out var cartData))
                {
                    shoppingCart = JsonConvert.DeserializeObject<List<CartDto>>(Encoding.UTF8.GetString(cartData));
                    //Kiểm tra xem có dữ liệu giỏ hàng đã được lưu trong Session không. Phương thức TryGetValue của
                    //Session được sử dụng để thử lấy dữ liệu từ khóa "ShoppingCart". Nếu dữ liệu tồn tại, biến cartData
                    //sẽ chứa dữ liệu của giỏ hàng.
                }
                else
                {
                    shoppingCart = new List<CartDto>();
                }
                ViewBag.Carts = shoppingCart;

                return View("Shop");
            }
            catch (HttpRequestException ex)
            {
                // Xử lý các lỗi liên quan đến yêu cầu HTTP
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Shop(int? pageNumber, int? categoryId, int? priceFrom, int? priceTo, string? keyWord, string? sortType)
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
            if(sortType == null)
            {
                sortType= "DateDesc";
            }

            try
            {
                itemPerPage = 12;
                HttpResponseMessage getAllProduct;
                if (categoryId == 0 && sortType != null)
                {
                    getAllProduct = await client.GetAsync(UrlApi + "Product?Page=" + pageNumber + "&PriceFrom=" + priceFrom + "&PriceTo=" + priceTo + "&ItemsPerPage=" + itemPerPage + "&SortType=" + sortType);
                }
                else if(keyWord != null)
                {
                    getAllProduct = await client.GetAsync(UrlApi + "Product?KeyWords=" + keyWord + "&Page=" + pageNumber + "&PriceFrom=" + priceFrom + "&PriceTo=" + priceTo + "&ItemsPerPage=" + itemPerPage);
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

                var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
                var getUser = await client.GetAsync(UrlApi + "Account/GetUser?Token=" + token);
                if (getUser.IsSuccessStatusCode)
                {
                    string content = await getUser.Content.ReadAsStringAsync();
                    var option = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    UserResponse user = System.Text.Json.JsonSerializer.Deserialize<UserResponse>(content, option);
                    ViewBag.User = user;
                }

                List<CartDto> shoppingCart;
                if (_httpContextAccessor.HttpContext.Session.TryGetValue("ShoppingCart", out var cartData))
                {
                    shoppingCart = JsonConvert.DeserializeObject<List<CartDto>>(Encoding.UTF8.GetString(cartData));
                    //Kiểm tra xem có dữ liệu giỏ hàng đã được lưu trong Session không. Phương thức TryGetValue của
                    //Session được sử dụng để thử lấy dữ liệu từ khóa "ShoppingCart". Nếu dữ liệu tồn tại, biến cartData
                    //sẽ chứa dữ liệu của giỏ hàng.
                }
                else
                {
                    shoppingCart = new List<CartDto>();
                }
                ViewBag.Carts = shoppingCart;

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
