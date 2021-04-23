using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagment.Models
{
    public class UserCommand
    {
        public int UserId { get; set; }
        public int CommandId { get; set; }
        public string Position { get; set; }
        public Command Command { get; set; }
        public UserProfile User { get; set; }
    }
}
