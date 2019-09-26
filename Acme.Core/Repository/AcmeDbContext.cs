using System;
using Acme.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Acme.Core.Repository
{
    public class AcmeDbContext : DbContext
    {
        public AcmeDbContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BusinessUnit>().HasData(new BusinessUnit
            {
                BusinessUnitId = 1,
                Name = "Cincinnati",
                City = "Cincinnati"
            });

            builder.Entity<Employee>().HasData(new Employee
            {
                EmployeeId = 1,
                FirstName = "Uncle",
                LastName = "Bob",
                Email = "uncle.bob@gmail.com",
                DateOfBirth = new DateTime(1979, 04, 25),
                PhoneNumber = "999-888-7777",
                BusinessUnitId = 1

            }, new Employee
            {
                EmployeeId = 2,
                FirstName = "Jan",
                LastName = "Kirsten",
                Email = "jan.kirsten@gmail.com",
                DateOfBirth = new DateTime(1981, 07, 13),
                PhoneNumber = "111-222-3333",
                BusinessUnitId = 1
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .AddUserSecrets<AcmeDbContext>()
                    .Build();
                optionsBuilder.UseSqlServer(configuration["DbConnectionString"], b => b.MigrationsAssembly("Acme.DataAccess"));
            }
            base.OnConfiguring(optionsBuilder);
        }
    }

}