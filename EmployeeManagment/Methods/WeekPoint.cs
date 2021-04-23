using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagment.Methods
{
    public class WeekPoint
    {
        public int AmountOfTasks { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public WeekPoint(int amount, DateTime start, DateTime end)
        {
            AmountOfTasks = amount;
            Start = start;
            End = end;
        }

    }
}
