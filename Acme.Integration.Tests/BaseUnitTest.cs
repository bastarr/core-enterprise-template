using System.Data.SqlClient;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Acme.Integration.Tests
{
    public abstract class BaseUnitTest
    {
        protected readonly IConfigurationRoot Configuration;
        protected readonly string DbConnection;
        public BaseUnitTest()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets<BaseUnitTest>()
                .Build();

            DbConnection = Configuration["DbConnectionString"];
        }

        public static void Init()
        {
        }

    }
}