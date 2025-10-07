using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FsElements.Data
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(SellerOf))]
        public string? SellerId { get; set; }

        [ForeignKey(nameof(BuyerOf))]
        public string? BuyerId { get; set; }
        public virtual FsUser? SellerOf { get; set; }
        public virtual FsUser? BuyerOf { get; set; }
        public List<OrderItem>? Items { get; set; }
        [Required]
        public string? BuyerPhone { get; set; }
        public string? Address { get; set; }
        
    }
}
