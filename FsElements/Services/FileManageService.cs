using Microsoft.AspNetCore.Components.Forms;

namespace FsElements.Services
{
    public interface IFileManageService
    {
        Task<string> SaveFile(IBrowserFile file, string folder);
    }

    public class FileManageService : IFileManageService
    {
        public async Task<string> SaveFile(IBrowserFile file, string folder)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(file.Name);
            var filePath = Path.Combine("Images", folder, fileName);
            using (var stream = File.Open(filePath, FileMode.OpenOrCreate))
            {
                using (var bStream = file.OpenReadStream(file.Size))
                {
                    await bStream.CopyToAsync(stream);
                }
            }
            return filePath;
        }
    }

    public static class FileFolders
    {
        public const string Forms = "Forms";
        public const string Elements = "Elements";
    }
}
