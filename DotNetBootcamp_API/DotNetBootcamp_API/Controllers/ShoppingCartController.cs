using DotNetBootcamp_API.Data;
using DotNetBootcamp_API.Models;
using DotNetBootcamp_API.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DotNetBootcamp_API.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        protected ApiResponse _response;
        private readonly ApplicationDbContext _db;
        public ShoppingCartController(ApplicationDbContext db)
        {
            _response = new();
            _db = db;
        }

        [HttpGet("get-cart")]
        public async Task<ActionResult<ApiResponse>> GetShoppingCart(string userId)
        {
            try
            {
                if(string.IsNullOrEmpty(userId))
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                // Trong cart thì chỉ có cart items, còn trong cart items thì mới có menu items nên phải dùng ThenInclude
                ShoppingCart shoppingCart = _db.ShoppingCarts.Include(u => u.CartItems)
                    .ThenInclude(u => u.MenuItem)
                    .FirstOrDefault(u=>u.UserId == userId);
                if(shoppingCart.CartItems != null && shoppingCart.CartItems.Count > 0)
                {
                shoppingCart.CartTotal = shoppingCart.CartItems.Sum(u => u.Quantity*u.MenuItem.Price);
                }
                _response.Result = shoppingCart;
                _response.StatusCode = HttpStatusCode.OK;
            } catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
                _response.StatusCode = HttpStatusCode.BadRequest;
            }
            return _response;
        }

        [HttpPost("add-to-cart")]
        public async Task<ActionResult<ApiResponse>> AddOrUpdateItemInCart(string userId, [FromBody] CartRequestDTO model)
        {
             //Shopping cart will have one entry per user's id if an user has many items in the cart.
             //Cart items will have all the items in shopping cart for a user
             //UpdateQuantityBy will have count by with an item quantity that need to be update
             //If it is -1 that means we have lower a count if it is 5 it means we have to add 5 count to existing count
             //If updateQuantityBy is 0, item will be removed


             //when users add a new item to a new shopping cart for the first time
             //when users add a new item to an existing shopping cart (basically users have other items in cart)
             //when users update an existing item count
             //when users remove an existing item

            ShoppingCart shoppingCart = _db.ShoppingCarts.Include(u=>u.CartItems).FirstOrDefault(u => u.UserId == userId);
            MenuItem menuItem = _db.MenuItems.FirstOrDefault(u => u.Id == model.menuItemId);
            if (menuItem == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Lỗi!");
                return NotFound(_response);
            }
            if(shoppingCart == null && model.updateQuantityBy > 0) {
                // Trường hợp 1: Người dùng chưa có giỏ hàng

                // Create a shopping cart for current user in the database & add cart item if the user hasn't add any items to the cart
                ShoppingCart newCart = new() { UserId = userId };
                _db.ShoppingCarts.Add(newCart);
                _db.SaveChanges();
                // Thêm sản phẩm đó vào giỏ dựa theo RequestDTO
                CartItem newCartItem = new()
                {
                    MenuItemId = model.menuItemId,
                    Quantity = model.updateQuantityBy,
                    ShoppingCartId = newCart.Id,
                    // If you don't set this to null, it will create a new MenuItem in the MenuItem table
                    MenuItem = null
                };
                _db.CartItems.Add(newCartItem);
                _db.SaveChanges();
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = shoppingCart;
                return Ok(_response);
            }
            else
            {
                
                // The user's already having a shopping cart?

                
                // Đây là lấy ra tất cả các cartItems hiện tại đang có trong giỏ hàng mà có MenuItemId trùng với id của cái MenuItem mình vừa thêm vào cart
                CartItem cartItemInCart = shoppingCart.CartItems.FirstOrDefault(u => u.MenuItemId == model.menuItemId);
                // Nếu sản phẩm đó hiện tại đang không có trong giỏ hàng
                if (cartItemInCart == null)
                {
                    // Thì ta sẽ tạo một cái CartItem mới trong giỏ hàng đó, với các thông tin người dùng truyền vào
                    CartItem newCartItem = new()
                    {
                        MenuItemId = model.menuItemId,
                        Quantity = model.updateQuantityBy,
                        ShoppingCartId = shoppingCart.Id,
                        // Nếu không set cái này là null thì sẽ lỗi
                        MenuItem = null
                    };
                    _db.CartItems.Add(newCartItem);
                    _db.SaveChanges();
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.IsSuccess = true;
                    _response.Result = shoppingCart;
                    return Ok(_response);
                } else
                {
                    // Trường hợp 2: Nếu sản phẩm đã có trong giỏ rồi

                    // Item already exist in the cart => we have to update the quantity
                    // Set a new quantity

                    // Nếu sản phẩm đó có sẵn trong giỏ hàng rồi thì mình chỉ việc update lại số lượng sản phẩm thôi
                    // SL sản phẩm mới = SL trước đó + SL người dùng vừa thêm vào
                    int newQuantity = cartItemInCart.Quantity + model.updateQuantityBy;
                    if(model.updateQuantityBy == 0 || newQuantity <= 0)
                    {
                        // Remove cart item from cart and if it is the only item then we'll remove the shopping cart as well
                        _db.CartItems.Remove(cartItemInCart);
                        if(shoppingCart.CartItems.Count() == 1)
                        {
                            _db.ShoppingCarts.Remove(shoppingCart);
                        }
                        _db.SaveChanges();
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.IsSuccess = true;
                        _response.Result = shoppingCart;
                        return Ok(_response);
                    }
                    else
                    {
                        cartItemInCart.Quantity = newQuantity;
                        _db.SaveChanges();
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.IsSuccess = true;
                        _response.Result = shoppingCart;
                        return Ok(_response);
                    }
                }
            }
        }
    }
}
