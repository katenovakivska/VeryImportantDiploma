using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagment.Models
{
    public class Command
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<UserCommand> UserCommands { get; set; }
        public int EmployerId { get; set; }
        public Command()
        {
            UserCommands = new List<UserCommand>();
        }
    }
}
