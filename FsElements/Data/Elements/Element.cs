using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FsElements.Data.Elements
{
    public class Element
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? UniqueCode { get; set; }

        [Required,StringLength(250)]
        public string? Name { get; set; }   
        
        public string? Image {  get; set; }

        [Required]
        public decimal PriceWholesale { get; set; }

        [Required]
        public decimal PriceRetail { get; set; }
                
        public double Width { get; set; }

        public double Height { get; set; }

        public double Weight { get; set; }

        [ForeignKey("ElementFormOf")]
        public int ElementFormId { get; set; }

        [ForeignKey("ElementCategoryOf")]
        public int CategoryId { get; set; }

        [ForeignKey("SellerOf")]
        public string? SellerId { get; set; }

        public virtual ElementCategory? ElementCategoryOf { get; set; }

        public virtual ElementForm? ElementFormOf { get; set; }

        public virtual FsUser? SellerOf { get; set; }
    }
}
