using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AspMvcAuth.Models;
using Microsoft.EntityFrameworkCore;

namespace AspMvcAuth.Data
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.Migrate();
        }
    }
}
