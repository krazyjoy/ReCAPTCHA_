using Microsoft.EntityFrameworkCore;

namespace ReCAPTCHA.Models
{
    public class ImageDbContext : DbContext
    {
        public ImageDbContext(DbContextOptions<ImageDbContext> options) : base(options)
        {

        }
        public DbSet<ImageModel> Images { get; set; }
    }
}
