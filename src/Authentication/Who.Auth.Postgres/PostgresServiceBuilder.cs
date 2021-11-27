using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Who.Auth.Context;

namespace Who.Auth.Postgres
{
    public static class PostgresServiceBuilder
    {
        public static void AddCoreDbContext(DbContextOptionsBuilder dbContextOptionsBuilder, string connectionString)
        {
            dbContextOptionsBuilder.UseNpgsql(connectionString,
                sql => sql.MigrationsAssembly(typeof(PostgresServiceBuilder).Assembly.FullName));
            dbContextOptionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.MultipleCollectionIncludeWarning));
        }

        public static void AddCoreDbContext(IServiceCollection serviceCollection, string connectionString)
        {

            serviceCollection.AddDbContext<AuthDbContext>(opt => opt.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(typeof(PostgresServiceBuilder).Assembly.FullName)));
        }
    }
}
