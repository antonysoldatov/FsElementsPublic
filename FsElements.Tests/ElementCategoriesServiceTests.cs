using FsElements.Data;
using FsElements.Data.Elements;
using FsElements.Services;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace FsElements.Tests;

public class ElementCategoriesServiceTests
{
    private readonly Mock<FsDbContext> _dbContextMock;
    private readonly Mock<IFileManageService> _fileManageService;
    private readonly ElementCategoriesService _service;

    public ElementCategoriesServiceTests()
    {
        var options = new DbContextOptionsBuilder<FsDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        _dbContextMock = new Mock<FsDbContext>(options) { CallBase = true };
        _fileManageService = new Mock<IFileManageService>();
        _fileManageService.Setup(f => f.SaveFile(It.IsAny<IBrowserFile>(), It.IsAny<string>())).Returns(Task.FromResult("test.png"));
        _service = new ElementCategoriesService(_dbContextMock.Object, _fileManageService.Object);
    }

    [Fact]
    public async Task GetAllCategories_ReturnsAllCategories()
    {
        // Arrange
        _dbContextMock.Object.ElementCategories.RemoveRange(_dbContextMock.Object.ElementCategories);
        _dbContextMock.Object.ElementCategories.Add(new ElementCategory { Name = "Cat1" });
        _dbContextMock.Object.ElementCategories.Add(new ElementCategory { Name = "Cat2" });
        await _dbContextMock.Object.SaveChangesAsync();

        // Act
        var result = await _service.GetAllCategories();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, c => c.Name == "Cat1");
        Assert.Contains(result, c => c.Name == "Cat2");
    }

    [Fact]
    public async Task AddCategory_AddsCategoryAndReturnsIt()
    {
        // Act
        var result = await _service.AddCategory("NewCat");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("NewCat", result.Name);
        Assert.True(result.Id > 0);
        Assert.Contains(_dbContextMock.Object.ElementCategories, c => c.Name == "NewCat");
    }

    [Fact]
    public async Task DeleteCategory_RemovesCategory_WhenExists()
    {
        // Arrange
        var category = await _service.AddCategory("NewCat");

        // Act
        await _service.DeleteCategory(category.Id);

        // Assert
        Assert.DoesNotContain(_dbContextMock.Object.ElementCategories, c => c.Id == category.Id);
    }

    [Fact]
    public async Task DeleteCategory_Throws_WhenNotFound()
    {
        // Act & Assert
        await Assert.ThrowsAsync<DataItemNotFoundException>(() => _service.DeleteCategory(999));
    }

    [Fact]
    public async Task GetCategoryById_ReturnsCategory_WhenExists()
    {
        // Arrange
        var category = await _service.AddCategory("FindMe");

        // Act
        var result = await _service.GetCategoryById(category.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("FindMe", result.Name);
        Assert.Equal(category.Id, result.Id);
    }

    [Fact]
    public async Task GetCategoryById_ReturnsNull_WhenNotFound()
    {
        // Act
        var result = await _service.GetCategoryById(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetFormsByCategoryId_ReturnsFormsForCategory()
    {
        // Arrange
        var category = await _service.AddCategory("FormCat");
        var form1 = await _service.AddForm("Form1", category.Id, Mock.Of<IBrowserFile>());
        var form2 = await _service.AddForm("Form2", category.Id, Mock.Of<IBrowserFile>());        

        // Act
        var result = await _service.GetFormsByCategoryId(category.Id);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, f => Assert.Equal(category.Id, f.ElementCategoryId));
    }

    [Fact]
    public async Task AddForm_AddsFormWithImage()
    {
        // Arrange
        var category = await _service.AddCategory("FormCat");
        var fileMock = new Mock<IBrowserFile>();

        // Act
        var result = await _service.AddForm("FormName", category.Id, fileMock.Object);

        // Assert        
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("FormName", result.Name);
        Assert.Equal(category.Id, result.ElementCategoryId);
        Assert.NotNull(result.Image);
    }

    [Fact]
    public async Task DeleteForm_RemovesForm_WhenExists()
    {
        // Arrange
        var form = new ElementForm { Name = "ToDeleteForm", Image = "img.png", ElementCategoryId = 1 };
        _dbContextMock.Object.ElementForms.Add(form);
        await _dbContextMock.Object.SaveChangesAsync();

        // Act
        await _service.DeleteForm(form.Id);

        // Assert
        Assert.DoesNotContain(_dbContextMock.Object.ElementForms, f => f.Id == form.Id);
    }

    [Fact]
    public async Task DeleteForm_Throws_WhenNotFound()
    {
        // Act & Assert
        await Assert.ThrowsAsync<DataItemNotFoundException>(() => _service.DeleteForm(999));
    }
}