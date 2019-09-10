using System;
using System.Linq;
using Acme.Business.Managers;
using Acme.Core.Repository;
using Acme.Core.Domain;
using Acme.DataAccess;
using Acme.DataAcess.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace Acme.Business.Tests
{
    [TestFixture]
    public class EmployeeManagerTests : BaseUnitTest
    {
        IUnitOfWork _unitOfWork;
        IRepository<Employee> _repository;
        IServiceProvider _serviceProvider;

        [SetUp]
        public void Setup()
        {
            // load mock static data from json
            var mockData = GetMockDbSet<Employee>(MockDataEntity.Employees);
            var mockContext = new Mock<AcmeDbContextExt>();
            mockContext.Setup(db => db.Set<Employee>()).Returns(mockData.Object);

            var services = new ServiceCollection();
            services.AddSingleton<IContext>(mockContext.Object);
            services.AddSingleton<IRepository<Employee>, EmployeeRepository>();
            services.AddSingleton<IUnitOfWork, UnitOfWork>();

            _serviceProvider = services.BuildServiceProvider();

            _unitOfWork = _serviceProvider.GetService<IUnitOfWork>();
            _repository = _unitOfWork.GetRepository<Employee>();
        }

        [Test]
        public void EmployeeManager_Get()
        {
            var manager = new EmployeeManager(_unitOfWork);
            var employees = manager.Get();

            Assert.IsNotNull(employees);
            Assert.IsTrue(employees.Count() == 2);
        }

        [Test]
        public void EmployeeManager_Get_By_Id()
        {
            var manager = new EmployeeManager(_unitOfWork);
            var actual = manager.Get(e => e.EmployeeId == 1).FirstOrDefault();

            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.EmployeeId);
            Assert.AreEqual(1, actual.BusinessUnitId);
        }
    }
}