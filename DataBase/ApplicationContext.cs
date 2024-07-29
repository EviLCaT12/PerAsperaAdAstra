using DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBase
{
    public class ApplicationContext : DbContext
    {
        public DbSet<NodeModel> Nodes { get; set; }
        public ApplicationContext (DbContextOptions options) : base (options) { }
        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            base.OnModelCreating (modelBuilder);

            modelBuilder.Entity<NodeModel>().HasIndex(model => model.Title);
        }
    } 
}