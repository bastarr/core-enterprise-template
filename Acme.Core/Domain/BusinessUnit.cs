using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Acme.Core.Domain
{
    public class BusinessUnit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long BusinessUnitId { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
