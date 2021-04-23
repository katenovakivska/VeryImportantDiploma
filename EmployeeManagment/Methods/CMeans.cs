using EmployeeManagment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagment.Methods
{
    public class CMeans
    {
        //оцінки характеристик об'єктів
        public int[,] SkillEvaluations { get; set; }
        //степені належності об'єктів кластерам
        public double[,] BelongingDegree { get; set; }
        //центроїди
        public double[,] Centroids { get; set; }
        //евклідові відстані
        public double[,] EuclidianDistances { get; set; }

        public CMeans(List<EmployeeSkill> skills, int amountOfSkills, int groups, int[,] skillsOfGroup)
        {
            var employees = skills.Select(x => x.EmployeeId).Distinct().OrderBy(x => x).ToList();
            var count = employees.Count;

            SkillEvaluations = new int[count, amountOfSkills];
            BelongingDegree = new double[count, groups];
            Centroids = new double[groups, amountOfSkills];
            EuclidianDistances = new double[count, groups];

            double startEvaluation = (double)1 / amountOfSkills;

            //пошук початкової належності об'єктів до кластерів
            foreach(var s in skills)
            {
                SkillEvaluations[employees.IndexOf(s.EmployeeId), s.SkillId - 1] = s.Mark;
                for (int i = 0; i < groups; i++)
                {
                    if (skillsOfGroup[i, s.SkillId - 1] != 0)
                    {
                        BelongingDegree[employees.IndexOf(s.EmployeeId), i] += startEvaluation;
                    }
                }
            }
        }
        public CMeans(int[,] skills, int amountOfSkills, int groups, int[,] skillsOfGroup)
        {
            //var employees = skills.Select(x => x.EmployeeId).Distinct().OrderBy(x => x).ToList();
            var count = skills.GetLength(0);

            SkillEvaluations = new int[count, amountOfSkills];
            BelongingDegree = new double[count, groups];
            Centroids = new double[groups, amountOfSkills];
            EuclidianDistances = new double[count, groups];
            for (int i = 0; i < skills.GetLength(0); i++)
            {
                for (int j = 0; j < skills.GetLength(1); j++)
                {
                    SkillEvaluations[i, j] = skills[i, j];
                }
            }
            double startEvaluation = (double)1 / amountOfSkills;

            //пошук початкової належності об'єктів до кластерів
            for (int i = 0; i < count; i++)
            {
                for (int k = 0; k < amountOfSkills; k++)
                {
                    for (int j = 0; j < groups; j++)
                    {
                        if (skillsOfGroup[j, k] != 0 && skills[i, k] != 0)
                        {
                            BelongingDegree[i, j] += startEvaluation;
                        }
                    }
                }
            }
        }
        //proverit
        public List<int> FindClaster()
        {
            List<int> clasters = new List<int>();
            bool canBeBetter = true;
            double[,] previousBelongingDegree = new double[BelongingDegree.GetLength(0), BelongingDegree.GetLength(1)];

            while (canBeBetter && BelongingDegree.Length != 0)
            {
                previousBelongingDegree = BelongingDegree;
                //пошук центроїдів
                for (int i = 0; i < Centroids.GetLength(0); i++)
                {
                    for (int j = 0; j < Centroids.GetLength(1); j++)
                    {
                        double sum = 0;
                        for (int k = 0; k < SkillEvaluations.GetLength(0); k++)
                        {
                            Centroids[i, j] += SkillEvaluations[k, j] * Math.Pow(BelongingDegree[k, i], 2);
                            sum += Math.Pow(BelongingDegree[k, i], 2);
                        }
                        Centroids[i, j] /= sum;
                    }
                }
                //розрахунок евклідових відстаней
                for (int i = 0; i < EuclidianDistances.GetLength(0); i++)
                {
                    for (int j = 0; j < EuclidianDistances.GetLength(1); j++)
                    {
                        double distance = 0;
                        for (int k = 0; k < Centroids.GetLength(1); k++)
                        {
                            distance += Math.Pow(SkillEvaluations[i, k] - Centroids[j, k], 2);
                        }
                        EuclidianDistances[i, j] = Math.Sqrt(distance);
                    }
                }
                //перерахунок степеней належності об'єктів до кластерів
                double[] distanceSum = new double[BelongingDegree.GetLength(0)];
                for (int i = 0; i < BelongingDegree.GetLength(0); i++)
                {
                    if (distanceSum[i] == 0)
                    {
                        for (int j = 0; j < BelongingDegree.GetLength(1); j++)
                        {
                            distanceSum[i] += 1 / EuclidianDistances[i, j];
                        }
                    }

                    for (int j = 0; j < BelongingDegree.GetLength(1); j++)
                    {
                        BelongingDegree[i, j] = (1 / EuclidianDistances[i, j]) / distanceSum[i];
                    }
                }
                canBeBetter = CheckIfCanBeImproved(previousBelongingDegree, BelongingDegree);
            }
            clasters = ChooseCluster();
            return clasters;
        }
        //перевірка виконання умови закінчення пошуку
        public bool CheckIfCanBeImproved(double[,] previousBelongingDegree, double[,] currentBelongingDegree)
        {
            bool canBeBetter = true;

            for (int i = 0; i < currentBelongingDegree.GetLength(0); i++)
            {
                for (int j = 0; j < currentBelongingDegree.GetLength(1); j++)
                {
                    if (Math.Abs(currentBelongingDegree[i, j] - previousBelongingDegree[i, j]) < 0.000001)
                    {
                        canBeBetter = false;
                        break;
                    }
                }
            }
            return canBeBetter;
        }
        //вибір найкращого класу для об'єкту
        public List<int> ChooseCluster()
        {
            List<int> clasters = new List<int>();
            for (int i = 0; i < BelongingDegree.GetLength(0); i++)
            {
                double max = 0;
                int index = 0;
                for (int j = 0; j < BelongingDegree.GetLength(1); j++)
                {
                    if (BelongingDegree[i, j] > max)
                    {
                        max = BelongingDegree[i, j];
                        index = j + 1;
                    }
                }
                clasters.Add(index);
            }
            return clasters;
        }

    }
}
