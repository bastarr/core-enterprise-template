using System.Data.SqlClient;
using System.Reflection;
using System.Threading.Tasks;
using Acme.Core.Domain;
using Acme.Core.Repository;
using Acme.DataAccess;
using Acme.DataAcess.Repositories;
using Acme.WebCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Acme.WebAPI
{
    public class Startup
    {
        private string _db;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureUserSecrets();

            services.AddDbContext<AcmeDbContext>(opts => opts.UseSqlServer(_db)); 
            services.AddScoped<IRepository<Employee>, EmployeeRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IContext, AcmeDbContextExt>();
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Acme WebAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            AutoMapperConfiguration.LoadAllMappingProfiles(Assembly.GetExecutingAssembly().FullName);

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Acme WebAPI v1");
            });

        }

        // Pull values from User Secrets and replace configuration
        private void ConfigureUserSecrets()
        {
            _db = Configuration["DbConnectionString"];
        }
    }
}
