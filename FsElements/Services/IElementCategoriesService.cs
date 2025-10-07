using FsElements.Data.Elements;
using Microsoft.AspNetCore.Components.Forms;

namespace FsElements.Services
{
    public interface IElementCategoriesService
    {        
        Task<ElementCategory> AddCategory(string name);
        Task<ElementForm> AddForm(string name, int categoryId, IBrowserFile file);
        Task DeleteCategory(int id);
        Task DeleteForm(int id);
        Task<List<ElementCategory>> GetAllCategories();
        Task<ElementCategory?> GetCategoryById(int id);
        Task<List<ElementForm>> GetFormsByCategoryId(int categoryId);
    }
}