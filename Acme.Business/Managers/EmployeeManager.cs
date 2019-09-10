using Acme.Core.Repository;
using Acme.Core.Domain;

namespace Acme.Business.Managers
{
    public class EmployeeManager : BaseManager<Employee>
    {
        public EmployeeManager(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}