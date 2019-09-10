using System;

namespace Acme.WebCore.Models
{
    public class EmployeeModel
    {
        public long EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public long BusinessUnitId { get; set; }
        public BusinessUnitModel BusinessUnit { get; set; }
    }
}