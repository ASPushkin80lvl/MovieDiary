using Microsoft.EntityFrameworkCore;
using MovieDiary.Library;

namespace MovieDiary
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Mark> Marks { get; set; }
    }
}
