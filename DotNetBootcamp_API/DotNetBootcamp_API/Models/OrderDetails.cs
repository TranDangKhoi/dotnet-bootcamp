using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetBootcamp_API.Models
{
    // many-one with OrderHeader
    // Mỗi một OrderDetail là để tượng trưng cho một sản phẩm mà người dùng đã order
    public class OrderDetails
    {
        [Key]
        public int OrderDetailId { get; set; }
        [Required]
        public int OrderHeaderId { get; set; }
        public int MenuItemId { get; set; }
        [ForeignKey("MenuItemId")]
        public MenuItem MenuItem { get; set; }
        [Required]
        // ?? Thực sự là có cần cái này khum
        public string ItemName { get; set; }
        [Required]
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}
