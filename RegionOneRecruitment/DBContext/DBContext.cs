using Microsoft.EntityFrameworkCore;
using RegionOneRecruitment.Models;

namespace RegionOneRecruitment.DBContext
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(_configuration.GetConnectionString("RegionOneDB"));
            }
        }

        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Opening> Openings { get; set; } = default!;
        public DbSet<Application> Applications { get; set; } = default!;
    }
}

