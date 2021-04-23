using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagment.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public int EmployerId { get; set; }
        public int EmployeeId { get; set; }
        public UserProfile Employer { get; set; }
    }
}
