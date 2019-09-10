using System.IO;
using Acme.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Acme.DataAccess
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AcmeDbContext>
    {
        AcmeDbContext IDesignTimeDbContextFactory<AcmeDbContext>.CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<AcmeDbContext>()
                .Build();
            var builder = new DbContextOptionsBuilder();
            builder.UseSqlServer(configuration["DbConnectionString"], b => b.MigrationsAssembly("Acme.DataAccess"));
            return new AcmeDbContext(builder.Options);
        }
    }
}