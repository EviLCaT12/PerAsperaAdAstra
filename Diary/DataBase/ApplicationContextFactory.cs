// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Design;
// using Microsoft.Extensions.Options;

// namespace DataBase
// {
//     public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
//     {
//         private readonly DataBaseCfgHelper _cfgHelper;
//         public ApplicationContextFactory(IOptions<DataBaseCfgHelper> cfgHelper)
//         {
//             _cfgHelper = cfgHelper.Value;            
//         }
//         public ApplicationContext CreateDbContext(string[] args)
//         {
//             var connectionString = $"Host={_cfgHelper.Host};Port={_cfgHelper.Port};Database={_cfgHelper.DataBase};Username={_cfgHelper.Username};Password={_cfgHelper.Password}";
//             var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
//             optionsBuilder.UseNpgsql(connectionString);
//             return new ApplicationContext(optionsBuilder.Options);
//         }
//     }
// }