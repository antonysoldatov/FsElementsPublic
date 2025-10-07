using FsElements.Data;
using FsElements.Data.Elements;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace FsElements.Services
{
    public class ElementsService : IElementsService
    {
        private readonly FsDbContext dbContext;
        private readonly IFileManageService fileManageService;

        public ElementsService(FsDbContext dbContext, IFileManageService fileManageService)
        {
            this.dbContext = dbContext;
            this.fileManageService = fileManageService;
        }

        public Task<List<Element>> GetElementsBySeller(string userId)
            => dbContext.Elements.Where(x => x.SellerId == userId)
                                 .Include(x => x.ElementFormOf)
                                 .OrderByDescending(x=>x.Id)
                                 .ToListAsync();

        public Task<Element?> GetElementById(int id)
            => dbContext.Elements.FirstOrDefaultAsync(x => x.Id == id);

        public async Task AddOrEdit(Element model, IBrowserFile? file)
        {
            if (file != null)
            {
                var fileName = await fileManageService.SaveFile(file, FileFolders.Elements);
                model.Image = fileName;
            }

            if (model.Id <= 0)
            {
                dbContext.Elements.Add(model);                
            }
            else
            {
                dbContext.Elements.Update(model);
            }
            await dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var item = await dbContext.Elements.FirstOrDefaultAsync(x => x.Id == id);
            if (item != null)
            {
                dbContext.Elements.Remove(item);
                await dbContext.SaveChangesAsync();
            }
            else
            {
                throw new DataItemNotFoundException();
            }
        }

        public async Task<List<Element>> GetElementsWithFilter(int categoryId, int formId)
        {
            return await dbContext.Elements.Where(x => (categoryId != 0 ? x.CategoryId == categoryId : true) &&
                                                        (formId != 0 ? x.ElementFormId == formId : true))
                                          .Include(x => x.ElementFormOf)
                                          .ToListAsync();
        }
    }
}
