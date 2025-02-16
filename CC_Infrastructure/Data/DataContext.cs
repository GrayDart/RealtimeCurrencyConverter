namespace CC_Infrastructure.Data
{
    using CC_Infrastructure.Model;
    using Microsoft.EntityFrameworkCore;
    using AppSettings_Reader;

    public class DataContext : DbContext
    {
        private readonly Helper _appSettings;

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            _appSettings = new Helper();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(_appSettings.GetConnectionString("SQLiteConnection"));
            }
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
    }
}