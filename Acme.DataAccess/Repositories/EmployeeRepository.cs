using Acme.Core.Domain;
using Acme.Core.Repository;
using System;
using System.Linq.Expressions;

namespace Acme.DataAcess.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IRepository<Employee>
    {
        public EmployeeRepository(IUnitOfWork unitOfWork) 
            : base(unitOfWork)
        {
            // Set default graph items for Get methods
            Expression<Func<Employee, object>>[] items = { i => i.BusinessUnit };
            DefaultGraphItems = items;
        }
    }

}