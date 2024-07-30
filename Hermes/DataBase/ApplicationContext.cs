using DataBase.Dbo;
using Microsoft.EntityFrameworkCore;

namespace DataBase;

public class ApplicationContext(DbContextOptions<ApplicationContext> options)
    : DbContext(options)
{
    public DbSet<OutboxDbo> Outboxes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
