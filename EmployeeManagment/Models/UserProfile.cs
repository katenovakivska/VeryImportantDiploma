using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagment.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Position { get; set; }
        public int CommandId { get; set; }
        public int Level { get; set; }
        public int AbilityToWorkInHome { get; set; }
        public int AbilityToWorkInOffice { get; set; }
        public double Experience { get; set; }
        public double AverageComplexityOfTasks { get; set; }
        public byte[] Picture { get; set; }
        [NotMapped]
        public string PictureConverted { get; set; }
        public ICollection<UserCommand> UserCommands { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public ICollection<EmployeeSkill> Skills { get; set; }
        public ICollection<Task> Tasks { get; set; }

        public UserProfile()
        {
            Employees = new List<Employee>();
            Skills = new List<EmployeeSkill>();
            Tasks = new List<Task>();
            UserCommands = new List<UserCommand>();
        }
    }
}
