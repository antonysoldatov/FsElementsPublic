using FsElements.Data.Elements;
using Microsoft.AspNetCore.Components.Forms;

namespace FsElements.Services
{
    public interface IElementsService
    {
        Task AddOrEdit(Element model, IBrowserFile? imageFile);
        Task Delete(int id);
        Task<Element?> GetElementById(int id);
        Task<List<Element>> GetElementsBySeller(string userId);
        Task<List<Element>> GetElementsWithFilter(int categoryId, int formId);
    }
}