using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagment.Models
{
    public class Task
    {
        public int Id { get; set; }
        public int CommandId { get; set; }
        public int CreatorId { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Status { get; set; }
        public int Complexity { get; set; }
        public int EmployeeId { get; set; }
        public UserProfile Employee { get; set; }
    }
}
