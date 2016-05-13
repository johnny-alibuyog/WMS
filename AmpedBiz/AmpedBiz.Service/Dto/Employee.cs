using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Service.Dto
{
    public class Employee
    {
        public string Id { get; set; }

        public User User { get; set; }

        public Contact Contact { get; set; }

        public string EmployeeTypeId { get; set; }

    }

    public class EmployeePageItem
    {
        public string Id { get; set; }

        public string EmployeeTypeName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }
        public Contact Contact { get; set; }

    }
}