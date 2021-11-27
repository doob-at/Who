using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Who.Auth.Context;
using Who.Auth.Postgres;
using Who.Auth.Sqlite;
using Who.Auth.SqlServer;

namespace Who.Auth.MigrationBuilder
{
    public class DesignTime : IDesignTimeDbContextFactory<AuthDbContext>
    {
        public AuthDbContext CreateDbContext(string[] args)
        {
            var serviceCollection = new ServiceCollection();

#if SQLITE
            SqliteServiceBuilder.AddCoreDbContext(serviceCollection, "Data Source=file.db");
#endif

#if SQLSERVER 
            SqlServerServiceBuilder.AddCoreDbContext(serviceCollection, "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = App");
#endif

#if POSTGRES
            PostgresServiceBuilder.AddCoreDbContext(serviceCollection, "Host=10.0.0.22;Database=App;Username=postgres;Password=postgres");
#endif


            var sp = serviceCollection.BuildServiceProvider();

            return sp.GetRequiredService<AuthDbContext>();
        }
    }
}
