using FsElements.Data;
using FsElements.Data.Elements;
using FsElements.Services;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsElements.Tests
{
    public class ElementsServiceTests
    {
        private readonly Mock<FsDbContext> _dbContextMock;
        private readonly Mock<IFileManageService> _fileManageService;
        private ElementsService _service;

        public ElementsServiceTests()
        {
            var options = new DbContextOptionsBuilder<FsDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb-" + Guid.NewGuid().ToString())
            .Options;
            _dbContextMock = new Mock<FsDbContext>(options) { CallBase = true };
            _dbContextMock.Object.ElementCategories.AddRange(new List<ElementCategory>
            {
                new ElementCategory { Id = 1, Name = "Category 1" },
                new ElementCategory { Id = 2, Name = "Category 2" }
            });
            _dbContextMock.Object.ElementForms.AddRange(new List<ElementForm>
            {
                new ElementForm { Id = 1, ElementCategoryId = 1, Name = "Form 1", Image="form1.png" },
                new ElementForm { Id = 2, ElementCategoryId = 1, Name = "Form 2", Image="form2.png" },
                new ElementForm { Id = 3, ElementCategoryId = 2, Name = "Form 3", Image="form3.png" }
            });
            _dbContextMock.Object.SaveChanges();

            _fileManageService = new Mock<IFileManageService>();
            _fileManageService.Setup(f => f.SaveFile(It.IsAny<IBrowserFile>(), It.IsAny<string>())).Returns(Task.FromResult("test.png"));
            _service = new ElementsService(_dbContextMock.Object, _fileManageService.Object);
        }

        [Fact]
        public async Task AddOrEdit_ShouldAddNewItem_WhenIdIsZero()
        {
            //Arrange
            _dbContextMock.Object.Elements.RemoveRange(_dbContextMock.Object.Elements);
            var elementName = "Test Element";
            var userId = "test_user";
            var uniqCode = Guid.NewGuid().ToString();
            var model = new Element
            {
                Id = 0,
                UniqueCode = uniqCode,
                Name = elementName,
                CategoryId = 1,
                ElementFormId = 1,
                PriceRetail = 100,
                PriceWholesale = 80,
                SellerId = userId
            };
            //Act
            await _service.AddOrEdit(model, Mock.Of<IBrowserFile>());
            //Assert
            Assert.True(model.Id > 0);
            var expectedModel = _dbContextMock.Object.Elements.FirstOrDefault(x => x.Id == model.Id);
            Assert.NotNull(expectedModel);
            Assert.Equal(elementName, expectedModel.Name);
        }

        [Fact]
        public async Task AddOrEdit_ShouldEditItem_WhenIdOfExistingItem()
        {
            //Arrange            
            _dbContextMock.Object.Elements.RemoveRange(_dbContextMock.Object.Elements);
            var elementName = "Test Element";
            var userId = "test_user";
            var uniqCode = Guid.NewGuid().ToString();
            var model = new Element
            {
                Id = 0,
                UniqueCode = uniqCode,
                Name = "Added Element",
                CategoryId = 1,
                ElementFormId = 1,
                PriceRetail = 1,
                PriceWholesale = 1,
                SellerId = userId
            };
            await _service.AddOrEdit(model, Mock.Of<IBrowserFile>());
            model.Name = elementName;
            model.PriceRetail = 100;
            model.PriceWholesale = 80;

            //Act
            await _service.AddOrEdit(model, null);

            //Assert
            var expectedModel = Assert.Single(_dbContextMock.Object.Elements.Where(x => x.Id == model.Id));
            Assert.Equal(elementName, expectedModel.Name);
            Assert.Equal(model.PriceRetail, expectedModel.PriceRetail);
            Assert.Equal(model.PriceWholesale, expectedModel.PriceWholesale);
        }

        [Fact]
        public async Task Delete_ShouldRemoveItem_WhenIdOfExistingItem()
        {
            //Arrange            
            _dbContextMock.Object.Elements.RemoveRange(_dbContextMock.Object.Elements);
            var userId = "test_user";
            var uniqCode = Guid.NewGuid().ToString();
            var model = new Element
            {
                Id = 0,
                UniqueCode = uniqCode,
                Name = "Added Element",
                CategoryId = 1,
                ElementFormId = 1,
                PriceRetail = 1,
                PriceWholesale = 1,
                SellerId = userId
            };
            await _service.AddOrEdit(model, Mock.Of<IBrowserFile>());
            //Act
            await _service.Delete(model.Id);
            //Assert
            var expectedModel = _dbContextMock.Object.Elements.FirstOrDefault(x => x.Id == model.Id);
            Assert.Null(expectedModel);
        }

        [Fact]
        public async Task Delete_ShouldThrowDataItemNotFoundException_WhenIdOfNonExistingItem()
        {
            //Arrange            
            _dbContextMock.Object.Elements.RemoveRange(_dbContextMock.Object.Elements);

            //Act & Assert
            await Assert.ThrowsAsync<DataItemNotFoundException>(() => _service.Delete(999));
        }

        [Fact]
        public async Task GetElementsBySeller_ShouldReturnItems_WhenIdOfExistingSeller()
        {
            //Arrange                        
            _dbContextMock.Object.Elements.RemoveRange(_dbContextMock.Object.Elements);
            var userId = "test_user";
            var uniqCode = Guid.NewGuid().ToString();
            var model1 = new Element
            {
                Id = 0,
                UniqueCode = uniqCode,
                Name = "Added Element 1",
                CategoryId = 1,
                ElementFormId = 1,
                PriceRetail = 1,
                PriceWholesale = 1,
                SellerId = userId
            };
            var model2 = new Element
            {
                Id = 0,
                UniqueCode = Guid.NewGuid().ToString(),
                Name = "Added Element 2",
                CategoryId = 1,
                ElementFormId = 1,
                PriceRetail = 1,
                PriceWholesale = 1,
                SellerId = userId
            };
            await _service.AddOrEdit(model1, Mock.Of<IBrowserFile>());
            await _service.AddOrEdit(model2, Mock.Of<IBrowserFile>());
            //Act           
            var result = await _service.GetElementsBySeller(userId);
            //Assert
            Assert.Equal(2, result.Count);
            Assert.Single(result.Where(x => x.Name == "Added Element 1"));
            Assert.Single(result.Where(x => x.Name == "Added Element 2"));
        }

        [Fact]
        public async Task GetElementById_ShouldReturnItem_WhenIdOfExistingItem()
        {
            //Arrange                        
            _dbContextMock.Object.Elements.RemoveRange(_dbContextMock.Object.Elements);
            var userId = "test_user";
            var uniqCode = Guid.NewGuid().ToString();
            var model = new Element
            {
                Id = 0,
                UniqueCode = uniqCode,
                Name = "Added Element 1",
                CategoryId = 1,
                ElementFormId = 1,
                PriceRetail = 1,
                PriceWholesale = 1,
                SellerId = userId
            };
            _dbContextMock.Object.Elements.Add(model);
            _dbContextMock.Object.SaveChanges();
            //Act           
            var result = await _service.GetElementById(model.Id);
            //Assert
            Assert.NotNull(result);
            Assert.Equal("Added Element 1", result.Name);
        }

        [Fact]
        public async Task GetElementById_ShouldReturnNull_WhenIdOfNonExistingItem()
        {
            //Arrange                        
            _dbContextMock.Object.Elements.RemoveRange(_dbContextMock.Object.Elements);
            //Act           
            var result = await _service.GetElementById(999);
            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetElementsWithFilter_ShouldReturnFilteredItems_WhenCategoryIdAndFormIdProvided()
        {
            //Arrange                        
            _dbContextMock.Object.Elements.RemoveRange(_dbContextMock.Object.Elements);
            var userId = "test_user";
            var model1 = new Element
            {
                Id = 0,
                UniqueCode = Guid.NewGuid().ToString(),
                Name = "Element 1",
                CategoryId = 1,
                ElementFormId = 1,
                PriceRetail = 1,
                PriceWholesale = 1,
                SellerId = userId
            };
            var model2 = new Element
            {
                Id = 0,
                UniqueCode = Guid.NewGuid().ToString(),
                Name = "Element 2",
                CategoryId = 1,
                ElementFormId = 2,
                PriceRetail = 1,
                PriceWholesale = 1,
                SellerId = userId
            };
            var model3 = new Element
            {
                Id = 0,
                UniqueCode = Guid.NewGuid().ToString(),
                Name = "Element 3",
                CategoryId = 2,
                ElementFormId = 3,
                PriceRetail = 1,
                PriceWholesale = 1,
                SellerId = userId
            };
            await _service.AddOrEdit(model1, Mock.Of<IBrowserFile>());
            await _service.AddOrEdit(model2, Mock.Of<IBrowserFile>());
            await _service.AddOrEdit(model3, Mock.Of<IBrowserFile>());
            //Act           
            var result = await _service.GetElementsWithFilter(1, 1);
            //Assert
            Assert.Single(result);
            Assert.Equal("Element 1", result[0].Name);
        }
    }
}
