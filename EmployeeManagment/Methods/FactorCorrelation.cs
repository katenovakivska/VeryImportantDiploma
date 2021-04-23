using EmployeeManagment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EmployeeManagment.Methods
{
    public class FactorCorrelation
    {
        public double[,] Factors { get; set; }
        public double[] Averages { get; set; }
        public double[,] Deviations { get; set; }
        public double[,] Dispersions { get; set; }
        public FactorCorrelation(List<UserProfile> employees)
        {
            Factors = new double[employees.Count(), 6];
            Averages = new double[6];
            Dispersions = new double[employees.Count(), 6];
            Deviations = new double[employees.Count(), 6];
            for (int i = 0; i < Factors.GetLength(0); i++)
            {
                Factors[i, 0] = employees[i].CommandId;
                Factors[i, 1] = employees[i].Level;
                Factors[i, 2] = employees[i].AbilityToWorkInHome;
                Factors[i, 3] = employees[i].AbilityToWorkInOffice;
                Factors[i, 4] = employees[i].Experience;
                Factors[i, 5] = employees[i].AverageComplexityOfTasks;
            }
        }

        public void AverageValues()
        {
            for (int i = 0; i < Averages.Length; i++)
            {
                for (int j = 0; j < Factors.GetLength(0); j++)
                {
                    Averages[i] += Factors[j, i];
                }
                Averages[i] /= Factors.GetLength(0);
            }
        }

        public double[] DeviationDispersionValues()
        {
            double[] sumOfDispersions = new double[6];
            for (int i = 0; i < Deviations.GetLength(0); i++)
            {
                for (int j = 0; j < Factors.GetLength(1); j++)
                {
                    Deviations[i, j] = Math.Abs(Averages[j] - Factors[i, j]);
                    Dispersions[i, j] = Math.Pow(Deviations[i, j], 2);
                    sumOfDispersions[j] += Dispersions[i, j];
                }
            }
            return sumOfDispersions;
        }

        public double[,] DeviationsMultiplication()
        {
            double[,] multiplication = new double[Deviations.GetLength(1), Deviations.GetLength(1)];
            for (int i = 0; i < Deviations.GetLength(1); i++)
            {
                for (int j = 0; j < Deviations.GetLength(1); j++)
                {
                    if (i == j)
                    {
                        multiplication[i, j] = 0;
                    }
                    else if (j > i)
                    {
                        for (int k = 0; k < Deviations.GetLength(0); k++)
                        {
                            multiplication[i, j] += Deviations[k, j] * Deviations[k, i];
                        }
                    }
                }
            }

            return multiplication;
        }

        public List<int> SelectNotSignificantFactors()
        {
            List<int> notSignificantIndexes = new List<int>();
            
            AverageValues();
            double[] sumOfDispersions = DeviationDispersionValues();
            double[,] multiplication = DeviationsMultiplication();
            double[] correlation = new double[Deviations.GetLength(1)];
            for (int i = 0; i < Deviations.GetLength(1); i++)
            {
                correlation[i] += (multiplication[0, i] + multiplication[i, 0]) / Math.Sqrt(sumOfDispersions[i] * sumOfDispersions[0]);
                if (correlation[i] < 0.75 && i != 0)
                {
                    notSignificantIndexes.Add(i - 1);
                }
            }
            return notSignificantIndexes;
        }

        public int[] OrderOfFactors()
        {
            List<int> notSignificantIndexes = new List<int>();

            AverageValues();
            double[] sumOfDispersions = DeviationDispersionValues();
            double[,] multiplication = DeviationsMultiplication();
            double[] correlation = new double[Deviations.GetLength(1) - 1];
            for (int i = 1; i < Deviations.GetLength(1); i++)
            {
                correlation[i - 1] += (multiplication[0, i] + multiplication[i, 0]) / Math.Sqrt(sumOfDispersions[i] * sumOfDispersions[0]);
            }
            int[] sortedIndexArray = correlation.Select((r, i) => new { Value = r, Index = i })
                            .OrderBy(t => t.Value)
                            .Select(p => p.Index)
                            .Reverse().ToArray();
            return sortedIndexArray;
        }
    }
}
