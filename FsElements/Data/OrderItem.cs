using FsElements.Data.Elements;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FsElements.Data
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Element))]
        public int ElementId { get; set; }

        public int Count { get; set; }

        public virtual Element? Element { get; set; }
    }
}
