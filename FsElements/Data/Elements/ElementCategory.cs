using System.ComponentModel.DataAnnotations;

namespace FsElements.Data.Elements
{
    public class ElementCategory
    {
        [Key]
        public int Id { get; set; }    
        [Required]
        public string? Name { get; set; }
    }
}
