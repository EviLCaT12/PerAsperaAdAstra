using DataBase.Dbo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DataBase
{
    public class ApplicationContext : DbContext
    {
        private readonly DataBaseCfgHelper _appConfig;

        public ApplicationContext(DbContextOptions<ApplicationContext> options, IOptions<DataBaseCfgHelper> config)
            : base(options)
        {
            _appConfig = config.Value;
        }

        public DbSet<OutboxDbo> Outboxes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = $"Host={_appConfig.Host};Port={_appConfig.Port};Database={_appConfig.DataBase};Username={_appConfig.Username};Password={_appConfig.Password}";
            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
