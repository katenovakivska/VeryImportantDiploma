using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagment.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public ICollection<Skill> Skills { get; set; }
        public Group()
        {
            Skills = new List<Skill>();
        }
    }
}
