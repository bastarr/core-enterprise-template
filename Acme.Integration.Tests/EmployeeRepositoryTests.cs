using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Acme.Core.Domain;
using Acme.Core.Repository;
using Acme.DataAccess;
using Acme.DataAcess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Acme.Integration.Tests
{
    [TestFixture]
    public class EmployeeRepositoryTests : BaseUnitTest
    {
        IServiceProvider _serviceProvider;
        IRepository<Employee> _repo;
        IUnitOfWork _uow;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddDbContext<AcmeDbContext>(opts => opts.UseSqlServer(DbConnection));
            services.AddSingleton<IRepository<Employee>, EmployeeRepository>();
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IContext, AcmeDbContextExt>();

            _serviceProvider = services.BuildServiceProvider();

            _uow = _serviceProvider.GetService<IUnitOfWork>();
            _repo = _uow.GetRepository<Employee>();
        }

        [Test]
        public void Employee_Repo_Get()
        {
            Assert.IsTrue(_repo.Get().Count() > 0);
        }

        [Test]
        public void Employee_Repo_Get_By_Id()
        {
            var actual = _repo.GetSingle(e => e.EmployeeId.Equals(1));

            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.EmployeeId);
            Assert.AreEqual(1, actual.BusinessUnit.BusinessUnitId);
        }

        [Test]
        public async Task Employee_Repo_Add()
        {
            var expected = await CreateEmployee();

            Assert.IsTrue(expected.EmployeeId > 0);
            Assert.AreEqual(1, expected.BusinessUnitId);
        }

        [Test]
        public async Task Employee_Repo_Update()
        {
            var expected = "111-222-3333";
            var employee = await CreateEmployee();
            employee.PhoneNumber = "111-222-3333";

            _repo.Update(employee);
            await _uow.SaveAsync();

            var actual = _repo.GetSingle(e => e.EmployeeId.Equals(employee.EmployeeId));

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.PhoneNumber);
            Assert.AreEqual(1, actual.BusinessUnit.BusinessUnitId);
        }

        [Test]
        public async Task Employee_Repo_Delete()
        {
            var employee = await CreateEmployee();
            _repo.Delete(employee);
            await _uow.SaveAsync();

            var actual = _repo.GetSingle(e => e.EmployeeId.Equals(employee.EmployeeId));

            Assert.IsNull(actual);
        }

        private async Task<Employee> CreateEmployee()
        {
            var expected = new Employee()
            {
                FirstName = "Int",
                LastName = "Test",
                DateOfBirth = new DateTime(1969, 9, 29),
                PhoneNumber = "555-555-5555",
                Email = "int.test@fakemail.com",
                BusinessUnitId = 1
            };

            await _repo.AddAsync(expected);
            await _uow.SaveAsync();

            return expected;
        }
    }
}