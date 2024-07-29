using DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBase
{
    public class ApplicationContext(DbContextOptions<ApplicationContext> options) 
        : DbContext(options)
    {
        public DbSet<NodeModel> Nodes { get; set; }
        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    } 
}