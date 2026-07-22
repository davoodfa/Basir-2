using Basir.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Basir.Infrastructure.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppIdentityDbContext>
{
    private const string ConnectionStringEnvironmentKey = "BasirConnection";
    private const string DefaultConnectionString =
        "Server=.;Database=BasirAuth;Trusted_Connection=True;TrustServerCertificate=True;";

    public AppIdentityDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable(ConnectionStringEnvironmentKey)
            ?? DefaultConnectionString;

        var builder = new DbContextOptionsBuilder<AppIdentityDbContext>();

        builder.UseSqlServer(connectionString, sql =>
        {
            sql.MigrationsHistoryTable("__EFMigrationsHistory", "Identity");
            sql.CommandTimeout(60);
        });

        return new AppIdentityDbContext(builder.Options);
    }
}
