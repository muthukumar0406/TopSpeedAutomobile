using Microsoft.EntityFrameworkCore;
using TopSpeed.web.Models;

namespace TopSpeed.web.data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { 
        }
        public DbSet<Brand> Brand {  get; set; }
    }
}
