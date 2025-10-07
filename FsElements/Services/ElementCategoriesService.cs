using FsElements.Data;
using FsElements.Data.Elements;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace FsElements.Services
{
    public class ElementCategoriesService : IElementCategoriesService
    {
        private readonly FsDbContext dbContext;
        private readonly IFileManageService fileManageService;

        public ElementCategoriesService(FsDbContext dbContext, IFileManageService fileManageService)
        {
            this.dbContext = dbContext;
            this.fileManageService = fileManageService;
        }

        public Task<List<ElementCategory>> GetAllCategories() => dbContext.ElementCategories.ToListAsync();

        public async Task<ElementCategory> AddCategory(string name)
        {
            var model = new ElementCategory
            {
                Name = name,
            };

            dbContext.ElementCategories.Add(model);
            await dbContext.SaveChangesAsync();
            return model;
        }

        public async Task DeleteCategory(int id)
        {
            var model = await dbContext.ElementCategories.FirstOrDefaultAsync(x => x.Id == id);
            if (model != null)
            {
                dbContext.ElementCategories.Remove(model);
                await dbContext.SaveChangesAsync();
            }
            else
            {
                throw new DataItemNotFoundException();
            }
        }

        public Task<ElementCategory?> GetCategoryById(int id) =>
            dbContext.ElementCategories.FirstOrDefaultAsync(x => x.Id == id);


        public Task<List<ElementForm>> GetFormsByCategoryId(int categoryId) =>
            dbContext.ElementForms.Where(x => x.ElementCategoryId == categoryId).ToListAsync();

        public async Task<ElementForm> AddForm(string name, int categoryId, IBrowserFile file)
        {
            var fileName = await fileManageService.SaveFile(file, FileFolders.Forms);
            var model = new ElementForm
            {
                Name = name,
                Image = fileName,
                ElementCategoryId = categoryId,
            };
            dbContext.ElementForms.Add(model);
            await dbContext.SaveChangesAsync();
            return model;
        }

        public async Task DeleteForm(int id)
        {
            var model = dbContext.ElementForms.FirstOrDefault(x => x.Id == id);
            if (model != null)
            {
                dbContext.ElementForms.Remove(model);
                await dbContext.SaveChangesAsync();
            }
            else
            {
                throw new DataItemNotFoundException();
            }
        }
    }
}
