using FsElements.Data;
using FsElements.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FsElements.Services
{
    public class OrderService : IOrderService
    {
        private readonly FsDbContext dbContext;
        private readonly IEmailService emailService;

        public OrderService(FsDbContext dbContext, IEmailService emailService)
        {
            this.dbContext = dbContext;
            this.emailService = emailService;
        }

        public async Task<bool> MakeOrder(List<ElementOrder> elements, string phoneNumber, string address)
        {
            if (elements == null || !elements.Any())
            {
                return false;
            }

            var order = new Order()
            {
                SellerId = elements.First().Element!.SellerId,
                BuyerPhone = phoneNumber,
                Address = address,
                Items = elements.Select(x => new OrderItem
                {
                    ElementId = x.Element!.Id,
                    Count = x.Count
                }).ToList()
            };

            dbContext.Orders.Add(order);
            await dbContext.SaveChangesAsync();

            var userSender = await dbContext.Users.SingleOrDefaultAsync(x => x.Id == order.SellerId!);

            var textElementList = "";
            foreach (var element in elements)
            {
                textElementList += $"({element.Element.UniqueCode}) {element.Element.Name}  x {element.Count}<br/>";
            }
            await emailService.SendEmailAsync(userSender.Email, "New Order",
                @$"You have new order: <br/>    
                    {textElementList} <br/>
                    Phone: {order.BuyerPhone} <br/>
                    Address: {order.Address}");

            return true;
        }

        public Task<List<Order>> GetOrdersBySellerId(string sellerId)
            => dbContext.Orders.Where(x => x.SellerId == sellerId)
                                .Include(x => x.Items!).ThenInclude(x => x.Element)
                                .ToListAsync();
    }
}
