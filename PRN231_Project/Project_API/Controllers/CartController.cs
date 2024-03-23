using System.Collections.Generic;
using BussinessObjects.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private static List<CartDto> ShoppingCart = new List<CartDto>();

        // Endpoint để lấy thông tin giỏ hàng
        [HttpGet]
        public ActionResult<IEnumerable<CartDto>> GetCart()
        {
            return Ok(ShoppingCart);
        }

        // Endpoint để thêm sản phẩm vào giỏ hàng
        [HttpPost]
        public ActionResult AddToCart(CartDto item)
        {
            ShoppingCart.Add(item);
            return Ok();
        }

        // Endpoint để cập nhật số lượng sản phẩm trong giỏ hàng
        [HttpPut]
        public ActionResult UpdateCartItem(int productId, int quantity)
        {
            var item = ShoppingCart.Find(i => i.ProductId == productId);
            if (item != null)
            {
                item.Quantity = quantity;
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        // Endpoint để xóa sản phẩm khỏi giỏ hàng
        [HttpDelete]
        public ActionResult RemoveFromCart(int productId)
        {
            var item = ShoppingCart.Find(i => i.ProductId == productId);
            if (item != null)
            {
                ShoppingCart.Remove(item);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
