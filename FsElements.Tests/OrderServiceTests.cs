using FsElements.Services;
using FsElements.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FsElements.Data;
using FsElements.Data.Elements;

namespace FsElements.Tests
{
    public class OrderServiceTests
    {
        private readonly Mock<FsDbContext> dbContextMock;
        private readonly Mock<IEmailService> emailServiceMock;
        private readonly OrderService orderService;
        private FsUser user;

        public OrderServiceTests()
        {
            var options = new DbContextOptionsBuilder<FsDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb-" + Guid.NewGuid().ToString())
            .Options;
            dbContextMock = new Mock<FsDbContext>(options) { CallBase = true };
            emailServiceMock = new Mock<IEmailService>();
            orderService = new OrderService(dbContextMock.Object, emailServiceMock.Object);

            user = new FsUser { Id = "seller1", Email = "seller@email.com" };
            dbContextMock.Object.Users.Add(user);
        }

        [Fact]
        public async Task MakeOrder_ReturnsFalse_WhenElementsIsNull()
        {
            // Act
            var result = await orderService.MakeOrder(null, "123", "address");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task MakeOrder_ReturnsFalse_WhenElementsIsEmpty()
        {
            // Act
            var result = await orderService.MakeOrder(new List<ElementOrder>(), "123", "address");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task MakeOrder_AddsOrderAndSendsEmail_WhenValidElements()
        {
            // Arrange            
            dbContextMock.Object.Orders.RemoveRange(dbContextMock.Object.Orders);
            var element = new Element { Id = 1, SellerId = "seller1", UniqueCode = "UC1", Name = "Element1" };
            var elementOrder = new ElementOrder { Element = element, Count = 2 };
            var elements = new List<ElementOrder> { elementOrder };
            emailServiceMock.Setup(x => x.SendEmailAsync(user.Email, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await orderService.MakeOrder(elements, "123", "address");

            // Assert
            Assert.True(result);            
            var orders = dbContextMock.Object.Orders.Where(o => o.SellerId == user.Id).ToList();
            Assert.Single(orders);
            emailServiceMock.Verify(x => x.SendEmailAsync(user.Email, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetOrdersBySellerId_ReturnsOrders()
        {
            // Arrange            
            dbContextMock.Object.Orders.RemoveRange(dbContextMock.Object.Orders);
            var element = new Element { Id = 1, SellerId = "seller1", UniqueCode = "UC1", Name = "Element1" };
            var elementOrder = new ElementOrder { Element = element, Count = 2 };
            var elements = new List<ElementOrder> { elementOrder };
            emailServiceMock.Setup(x => x.SendEmailAsync(user.Email, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            await orderService.MakeOrder(elements, "123", "address");

            // Act
            var result = await orderService.GetOrdersBySellerId(user.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(user.Id, result.First().SellerId);
        }
    }
}
