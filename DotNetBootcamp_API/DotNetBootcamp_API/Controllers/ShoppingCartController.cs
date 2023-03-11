using DotNetBootcamp_API.Data;
using DotNetBootcamp_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetBootcamp_API.Controllers
{
    [Route("api/[controller]")]
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

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddOrUpdateItemInCart(string userId, int menuItemId, int updateQuantity)
        {
            // Shopping cart will have one entry per user's id if an user has many items in the cart.
            // Cart items will have all the items in shopping cart for a user
            // UpdateQuantityBy will have count by with an item quantity that need to be update
            // If it is -1 that means we have lower a count if it is 5 it means we have to add 5 count to existing count
            // If updateQuantityBy is 0, item will be removed
        
            
            // when users add a new item to a new shopping cart for the first time
            // when users add a new item to an existing shopping cart (basically users have other items in cart)
            // when users update an existing item count
            // when users remove an existing item
        
        }
    }
}
