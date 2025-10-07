using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FsElements.Data.Elements
{
    public class ElementForm
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        [Required]
        public string? Image { get; set; }

        [ForeignKey("ElementCategoryOf")]
        public int? ElementCategoryId { get; set; }

        public virtual ElementCategory? ElementCategoryOf { get; set; }
    }
}
