using System.ComponentModel;

namespace FsElements.Models
{
    public class OrderBasket
    {
        public event EventHandler<int>? CountChanged;
        public List<ElementOrder> ElementOrders { get; set; } = new List<ElementOrder>();

        public int Count { get =>  ElementOrders.Count; }

        public void NotifyChanged()
        {
            CountChanged?.Invoke(this, Count);
        }
    }
}
