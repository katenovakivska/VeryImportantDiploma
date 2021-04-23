using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagment.Methods
{
    public class Efficiency
    {
        public int Amount { get; set; }
        public double Value1 { get; set; }
        public double Value2 { get; set; }
        public double Value3 { get; set; }
        public double Value4 { get; set; }
        public double Avg { get; set; }
        public double SpeedUp { get; set; }

        public Efficiency(int amount, double value1, double value2)
        {
            Amount = amount;
            Value1 = value1;
            Value2 = value2;
            SpeedUp = Math.Round(value1/value2, 3);
        }
        public Efficiency(int amount, double value1, double value2, double value3, double value4)
        {
            Amount = amount;
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
            Value4 = value4;
            Avg = (value1 + value2 + value3 + value4) / 4;
        }
    }
}
