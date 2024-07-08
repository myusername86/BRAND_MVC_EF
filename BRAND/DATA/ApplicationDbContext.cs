using Microsoft.EntityFrameworkCore;
using BRAND.Models;

namespace BRAND.DATA
{
    public class ApplicationDbContext:DbContext
    {
        //creating constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options) 
        {
            
        }

        public DbSet<Brand> Brand { get; set; }
    }
}
