using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagment.Models
{
    [NotMapped]
    public class UserSkillModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Login { get; set; }
        public int[] SkillEvaluations { get; set; }
        public string Position { get; set; }
        public string NewPosition { get; set; }
        public UserSkillModel(int groups)
        {
            SkillEvaluations = new int[groups];
        }
    }
}
