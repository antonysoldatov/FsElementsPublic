using FsElements.Data;
using FsElements.Models;

namespace FsElements.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrdersBySellerId(string sellerId);
        Task<bool> MakeOrder(List<ElementOrder> elements, string phoneNumber, string address);
    }
}