using EmployeeManagment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace EmployeeManagment.Methods
{
    public class BayesClassifier
    {
        public List<UserProfile> Employees { get; set; }
        public List<List<double>> EmployeesWithoutTeam { get; set; }
        public List<List<double>> PosteriorNumbers { get; set; }
        public List<int> NotSignificantColumns { get; set; }
        public List<int> Commands { get; set; }
        public double[,] Averages { get; set; }
        public double[,] Dispersions { get; set; }
        public double[,] Deviations { get; set; }

        public BayesClassifier(List<UserProfile> employees, List<UserProfile> employee, List<int> commands)
        {
            EmployeesWithoutTeam = new List<List<double>>();
            for (int i = 0; i < employee.Count(); i++)
            {
                List<double> list = new List<double>();
                list.Add(employee[i].Level + 1);
                list.Add(employee[i].AbilityToWorkInHome + 1);
                list.Add(employee[i].AbilityToWorkInOffice + 1);
                list.Add(employee[i].Experience + 1);
                list.Add(employee[i].AverageComplexityOfTasks + 1);
                EmployeesWithoutTeam.Add(list);
            }
            Employees = employees;
            FactorCorrelation factorCorrelation = new FactorCorrelation(Employees);
            NotSignificantColumns = factorCorrelation.SelectNotSignificantFactors();
            PosteriorNumbers = new List<List<double>>();
            Commands = commands;
            Averages = new double[commands.Count(), 5];
            Dispersions = new double[commands.Count(), 5];
            Deviations = new double[commands.Count(), 5];
        }

        public void FindCommands()
        {
            int amount = Employees.Where(x => x.CommandId != 0).Select(x => x.CommandId).Distinct().Count();

            for (int i = 0; i < Commands.Count(); i++)
            {
               //CommandStatistics command = new CommandStatistics(Commands[i]);
                int count = Employees.Where(x => x.CommandId == Commands[i]).Count();
                //средние и дисперсии
                Averages[i, 0] = (double)(Employees.Where(x => x.CommandId == Commands[i]).Sum(x => x.Level) + count) / count;
                Averages[i, 1] = (double)(Employees.Where(x => x.CommandId == Commands[i]).Sum(x => x.AbilityToWorkInHome) + count) / count;
                Averages[i, 2] = (double)(Employees.Where(x => x.CommandId == Commands[i]).Sum(x => x.AbilityToWorkInOffice) + count) / count;
                Averages[i, 3] = (double)(Employees.Where(x => x.CommandId == Commands[i]).Sum(x => x.Experience) + count) / count;
                Averages[i, 4] = (double)(Employees.Where(x => x.CommandId == Commands[i]).Sum(x => x.AverageComplexityOfTasks) + count) / count;

                Dispersions[i, 0] = (double)Employees.Where(x => x.CommandId == Commands[i]).Sum(x => Math.Pow(x.Level + 1 - Averages[i, 0], 2)) / (count - 1);
                Dispersions[i, 1] = (double)Employees.Where(x => x.CommandId == Commands[i]).Sum(x => Math.Pow(x.AbilityToWorkInHome + 1 - Averages[i, 1], 2)) / (count - 1);
                Dispersions[i, 2] = (double)Employees.Where(x => x.CommandId == Commands[i]).Sum(x => Math.Pow(x.AbilityToWorkInOffice + 1 - Averages[i, 2], 2)) / (count - 1);
                Dispersions[i, 3] = (double)Employees.Where(x => x.CommandId == Commands[i]).Sum(x => Math.Pow(x.Experience + 1 - Averages[i, 3], 2)) / (count - 1);
                Dispersions[i, 4] = (double)Employees.Where(x => x.CommandId == Commands[i]).Sum(x => Math.Pow(x.AverageComplexityOfTasks + 1 - Averages[i, 4], 2)) / (count - 1);

                //средние квадратические отклонения
                for (int j = 0; j < Deviations.GetLength(1); j++)
                {
                    if (!NotSignificantColumns.Contains(j))
                    {
                        Deviations[i, j] = Dispersions[i, j] / Averages[i, j];
                    }
                    else
                    {
                        Deviations[i, j] = -1;
                    }
                }
            }
        }

        public List<int> FindPosteriorNumerator()
        {
            List<int> groups = new List<int>();
            for (int k = 0; k < EmployeesWithoutTeam.Count(); k++)
            {
                for (int i = 0; i < Commands.Count(); i++)
                {
                    List<double> posteriors = new List<double>();
                    //апостериорные числители
                    posteriors.Add((double)Employees.Where(x => x.CommandId == Commands[i]).Count() / Employees.Count());

                    for (int j = 0; j < Deviations.GetLength(1); j++)
                    {
                        if (Deviations[i, j] != -1)
                        {
                            posteriors.Add((1 / Math.Sqrt(2 * Math.PI * Deviations[i, j]))
                            * Math.Exp(-Math.Pow(EmployeesWithoutTeam[k].ElementAt(j) - Dispersions[i, j], 2) / (2 * Deviations[i, j])));
                        }
                    }
                    PosteriorNumbers.Add(posteriors);
                }
                List<int> forDelete = new List<int>();
                for (int i = 0; i < Commands.Count(); i++)
                {
                    for (int j = 0; j < PosteriorNumbers[0].Count(); j++)
                    {
                        if (Double.IsNaN(PosteriorNumbers[i].ElementAt(j)))
                        {
                            forDelete.Add(j);
                        }
                    }
                }
                forDelete = forDelete.OrderByDescending(x => x).ToList();
                forDelete = forDelete.Distinct().ToList();
                for (int t = 0; t < Commands.Count(); t++)
                {
                    for (int i = 0; i < forDelete.Count(); i++)
                    {
                        PosteriorNumbers[t].RemoveAt(forDelete[i]);
                    }
                }
                double max = 0;
                int index = 0;
                List<double> multiplePosteriors = new List<double>();
                for (int i = 0; i < PosteriorNumbers.Count(); i++)
                {
                    double val = 1;
                    for (int j = 0; j < PosteriorNumbers[i].Count; j++)
                    {
                        val *= PosteriorNumbers[i].ElementAt(j);
                    }
                    multiplePosteriors.Add(val);
                }
                    //поиск наиболее подходящего числителя
               for (int i = 0; i < multiplePosteriors.Count(); i++)
                {
                    if (multiplePosteriors[i] > max)
                    {
                        max = multiplePosteriors[i];
                        index = i;
                    }
                }
                groups.Add(Commands[index]);
                PosteriorNumbers = new List<List<double>>();
            }
            return groups;
        }
    }
}
