using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FsElements.Data.Elements;

namespace FsElements.Data
{
    public class FsDbContext(DbContextOptions<FsDbContext> options) : IdentityDbContext<FsUser>(options)
    {
        public virtual DbSet<ElementCategory> ElementCategories { get; set; }
        public virtual DbSet<ElementForm> ElementForms { get; set; }
        public virtual DbSet<Element> Elements { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
    }
}
