using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagment.Methods
{
    public class CheckForecastResult
    {
        public double[] History { get; set; }
        public double[] Forecast1 { get; set; }
        public double[] Forecast2 { get; set; }
        public double Deviation { get; set; }

        public CheckForecastResult(double[] history, double[] forecast1, double[] forecast2, double deviation)
        {
            History = history;
            Forecast1 = forecast1;
            Forecast2 = forecast2;
            Deviation = Math.Round(deviation, 3);
            for (int i = 0; i < Forecast1.Length; i++)
            {
                Forecast1[i] = Math.Round(Forecast1[i], 3);
                Forecast2[i] = Math.Round(Forecast2[i], 3);
            }
            for (int i = 0; i < History.Length; i++)
            {
                if (History[i] / Math.Round(History[i], 0) != 1)
                {
                    History[i] = Math.Round(History[i], 3);
                }
            }
        }

    }
}
