using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetBootcamp_API.Models
{
    // one-many with OrderDetail
    // một OrderHeader sẽ chứa NHIỀU OrderDetails, mỗi OrderDetails sẽ tượng trưng cho một sản phẩm
    public class OrderHeader
    {
        [Key]
        public int OrderHeaderId { get; set; }
        // Người order tên là gì ??
        [Required]
        public string PickupName { get; set; }
        // Số điện thoại của anh/cô ấy là gì??
        [Required]
        public string PickupPhoneNumber { get; set; }
        // Email là gì ??
        [Required]
        public string PickupEmail { get; set; }
        // Là thằng user nào đã đặt cái order này
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        // Số lượng sản phẩm mà nó đặt là bao nhiêu (tính số lượng OrderDetails)
        public double OrderTotal { get; set; }
        // Ngày đặt ??
        public DateTime OrderDate { get; set; }
        public string StripePaymentIntentID { get; set; }
        public string Status { get; set; }
        public int TotalItems { get; set; }
    
        public IEnumerable<OrderDetails> OrderDetails { get; set; }
    }
}
