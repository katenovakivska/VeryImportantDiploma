using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagment.Methods
{
    public class TypeStatistics
    {
        public string Type { get; set; }
        public double Precision { get; set; }
        public int Uncorrect { get; set; }
        public int Total { get; set; }

        public TypeStatistics(string type, double precision, int uncorrect, int total)
        {
            Type = type;
            Precision = Math.Round(precision, 3);
            Uncorrect = uncorrect;
            Total = total;
        }
    }
}
