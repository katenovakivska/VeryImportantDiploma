using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagment.Methods;
using EmployeeManagment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Newtonsoft.Json;
using Extreme.DataAnalysis;
using Extreme.Mathematics;
using Extreme.Statistics;
using Extreme.Statistics.TimeSeriesAnalysis;
using System.Diagnostics;

namespace EmployeeManagment.Controllers
{
    public class CheckController : Controller
    {
        EmployeeContext db;
        const string SessionName = "_Name";
        const string SessionId = "_Id";
        public CheckController(EmployeeContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult CheckForecast(int commandId, int amountOfWeeks = 3)
        {
            var tasks = db.Task.Where(t => t.CommandId == commandId).OrderBy(t => t.End).ToList();
            var startDate = tasks[0].Start;
            var endDate = startDate.AddDays(7);
            List<DataPoint> points = new List<DataPoint>();
            var cal = new GregorianCalendar();
            var startWeek = cal.GetWeekOfYear(startDate, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            var endWeek = cal.GetWeekOfYear(tasks.Last().End, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            var counter = endWeek - startWeek;
            if (startDate.Year == endDate.Year)
            {
                for (int i = 0; i < counter; i++)
                {
                    var amount = db.Task.Where(t => t.CommandId == commandId && t.End >= startDate && t.End <= endDate).Count();
                    double avgComplexity = 0;
                    if (amount != 0)
                    {
                        avgComplexity = (double)(db.Task.Where(t => t.CommandId == commandId && t.End >= startDate && t.End <= endDate).Sum(x => x.Complexity) / amount);
                    }
                    points.Add(new DataPoint(amount, endDate, avgComplexity));
                    startDate = startDate.AddDays(7);
                    endDate = endDate.AddDays(7);
                }
            }
            else
            {
                var years = endDate.Year - startDate.Year + 1;

                for (int j = 0; j < years; j++)
                {
                    if (j == 0)
                    {
                        for (int i = 0; i < counter; i++)
                        {
                            var amount = db.Task.Where(t => t.CommandId == commandId && t.End >= startDate && t.End <= endDate).Count();
                            var avgComplexity = (double)(db.Task.Where(t => t.CommandId == commandId && t.End >= startDate && t.End <= endDate).Sum(x => x.Complexity) / amount);
                            points.Add(new DataPoint(amount, endDate, avgComplexity));
                            startDate.AddDays(7);
                            endDate.AddDays(7);
                        }
                    }
                    else if (j == years - 1)
                    {
                        for (int i = 0; i < endWeek; i++)
                        {
                            var amount = db.Task.Where(t => t.CommandId == commandId && t.End >= startDate && t.End <= endDate).Count();
                            var avgComplexity = (double)(db.Task.Where(t => t.CommandId == commandId && t.End >= startDate && t.End <= endDate).Sum(x => x.Complexity) / amount);
                            points.Add(new DataPoint(amount, endDate, avgComplexity));
                            startDate.AddDays(7);
                            endDate.AddDays(7);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 52; i++)
                        {
                            var amount = db.Task.Where(t => t.CommandId == commandId && t.End >= startDate && t.End <= endDate).Count();
                            var avgComplexity = (double)(db.Task.Where(t => t.CommandId == commandId && t.End >= startDate && t.End <= endDate).Sum(x => x.Complexity) / amount);
                            points.Add(new DataPoint(amount, endDate, avgComplexity));
                            startDate.AddDays(7);
                            endDate.AddDays(7);
                        }
                    }
                }
            }
            var amountArray = points.Select(x => Convert.ToDouble(x.Amount)).ToList();
            var complexityArray = points.Select(x => x.AvgComplexity).ToList();
            var historyComplexity = Vector.Create(complexityArray.ToArray());
            var historyAmount = Vector.Create(amountArray.ToArray());
            ArimaModel model = new ArimaModel(historyAmount, 2, 1);
            model.Compute();
            var forecast = model.Forecast(amountOfWeeks);
            ArimaModel model1 = new ArimaModel(historyComplexity, 2, 1);
            model1.Compute();
            var forecast1 = model1.Forecast(amountOfWeeks);
            List<DataPoint> forecastPoints = new List<DataPoint>();
            var lastDate = new DateTime(points.Last().Year, points.Last().Month, points.Last().Day);
            lastDate = lastDate.AddDays(1);

            List<DataPoint> forecastPoints1 = new List<DataPoint>();
            var model2 = new ExponentialSmoothingModel(historyAmount, ExponentialSmoothingMethod.Double);
            model2.TrendEstimator = ExponentialSmoothingTrendEstimator.Complete;
            model2.Compute();
            var forecast2 = model2.Forecast(amountOfWeeks);
            var model3 = new ExponentialSmoothingModel(historyComplexity, ExponentialSmoothingMethod.Double);
            model3.Compute();
            var forecast3 = model3.Forecast(amountOfWeeks);

            forecastPoints.Add(new DataPoint(points.Last().Amount, lastDate, points.Last().AvgComplexity));
            forecastPoints1.Add(new DataPoint(points.Last().Amount, lastDate, points.Last().AvgComplexity));
            for (int i = 0; i < forecast.Count(); i++)
            {
                forecastPoints.Add(new DataPoint(forecast[i], endDate, forecast1[i]));
                forecastPoints1.Add(new DataPoint(forecast2[i], endDate, forecast3[i]));
                endDate = endDate.AddDays(7);
            }
            Random random = new Random();


            double deviation = 0, deviation1 = 0, deviation2 = 0;
            List<CheckForecastResult> amounts = new List<CheckForecastResult>();
            List<CheckForecastResult> complexities = new List<CheckForecastResult>();
            List<CheckForecastResult> productivities = new List<CheckForecastResult>();

            for (int j = 0; j < 50; j++)
            {
                double[] amountValues = new double[20];
                double[] complexityValues = new double[20];
                double[] productivityValues = new double[20];
                for (int i = 0; i < 20; i++)
                {
                    amountValues[i] = random.Next(1, 15);
                    complexityValues[i] = random.Next(1, 8) + random.NextDouble();
                    productivityValues[i] = amountValues[i] * complexityValues[i];
                }
                ArimaModel model4 = new ArimaModel(amountValues, 2, 1);
                model4.Compute();
                var check = model4.Forecast(amountOfWeeks);

                var model5 = new ExponentialSmoothingModel(amountValues, ExponentialSmoothingMethod.Double);
                model5.TrendEstimator = ExponentialSmoothingTrendEstimator.Complete;
                model5.Compute();
                var check1 = model5.Forecast(amountOfWeeks);

                ArimaModel model6 = new ArimaModel(complexityValues, 2, 1);
                model6.Compute();
                var check2 = model6.Forecast(amountOfWeeks);

                var model7 = new ExponentialSmoothingModel(complexityValues, ExponentialSmoothingMethod.Double);
                model7.TrendEstimator = ExponentialSmoothingTrendEstimator.Complete;
                model7.Compute();
                var check3 = model7.Forecast(amountOfWeeks);
                double devA = 0, devC = 0, devP = 0;
                double[] check4 = new double[check.Count];
                double[] check5 = new double[check.Count];
                for (int k = 0; k < check.Count; k++)
                {
                    devA += Math.Abs(check[k] - check1[k]);
                    devC += Math.Abs(check2[k] - check3[k]);
                    devP += Math.Abs((check[k]*check2[k]) - (check3[k]*check1[k]));
                    check4[k] = check[k] * check2[k];
                    check5[k] = check3[k] * check1[k];
                }
                deviation += devA;
                deviation1 += devC;
                deviation2 += devP;
                amounts.Add(new CheckForecastResult(amountValues, check.ToArray(), check1.ToArray(), devA / check.Count));
                complexities.Add(new CheckForecastResult(complexityValues, check2.ToArray(), check3.ToArray(), devC / check.Count));
                productivities.Add(new CheckForecastResult(productivityValues, check4, check5, devP / check.Count));
            }
            ViewBag.TimeArima = FillEfficiencyArima();
            ViewBag.TimeExp = FillEfficiencyExponential();
            ViewBag.Efficiency = FillEfficiencyForecast();
            deviation /= (50 * amountOfWeeks);
            deviation1 /= (50 * amountOfWeeks);
            deviation2 /= (50 * amountOfWeeks);
            ViewBag.Deviation = Math.Round(deviation, 0);
            ViewBag.Deviation1 = Math.Round(deviation1, 0);
            ViewBag.Deviation2 = Math.Round(deviation2, 0);
            JsonSerializerSettings jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
            ViewBag.DataPoints = JsonConvert.SerializeObject(points, jsonSetting);
            ViewBag.ForecastPoints = JsonConvert.SerializeObject(forecastPoints, jsonSetting);
            ViewBag.ForecastPoints1 = JsonConvert.SerializeObject(forecastPoints1, jsonSetting);
            ViewBag.CommandId = commandId;
            ViewBag.Amounts = amounts;
            ViewBag.Complexities = complexities;
            ViewBag.Productivities = productivities;
            return View();
        }


        [HttpGet]
        public IActionResult CheckPositions()
        {
            List<UserSkillModel> users = new List<UserSkillModel>();
            users = CreateSetForCMeansCheck();
            int[,] skills = new int[users.Count, db.Skills.Count()];
            for (int i = 0; i < users.Count; i++)
            {
                for (int j = 0; j < db.Skills.Count(); j++)
                {
                    skills[i, j] = users[i].SkillEvaluations[j];
                }
            }
            var skillsOfGroup = new int[db.Groups.Count(), db.Skills.Count()];
            foreach (var s in db.Skills)
            {
                skillsOfGroup[s.GroupId - 1, s.Id - 1] = 1;
            }
            CMeans cMeans = new CMeans(skills, db.Skills.Count(), db.Groups.Count(), skillsOfGroup);
            List<int> newGroups = cMeans.FindClaster();
            int k = 0;
            ViewBag.Efficiency = FillEfficiencyPosition();
            foreach (var e in users)
            {
                e.NewPosition = db.Groups.Where(x => x.Id == newGroups[k]).Select(x => x.GroupName).FirstOrDefault();
                k++;
            }
            List<TypeStatistics> statistics = new List<TypeStatistics>();
            var positions = users.Select(x => x.Position).Distinct().ToList();
            int positionsCounter = positions.Count();
            for (int i = 0; i < positionsCounter; i++)
            {
                int trueDetected = users.Where(x => x.Position == positions[i] && x.NewPosition == x.Position).Count();
                int falseDetected = users.Where(x => x.Position == positions[i] && x.NewPosition != x.Position).Count();
                int allInType = users.Where(x => x.Position == positions[i]).Count();
                statistics.Add(new TypeStatistics(positions[i], (double)trueDetected / allInType, falseDetected, allInType));
            }
            ViewBag.UserSkills = users;
            ViewBag.SkillNames = db.Skills.Select(x => x.SkillName).ToList();
            int uncorrect = users.Where(x => x.Position != x.NewPosition).Count();
            double procent = uncorrect / users.Count();
            ViewBag.Uncorrect = uncorrect;
            ViewBag.Procent = procent;
            ViewBag.Statistics = statistics;
            return View();
        }

        [HttpGet]
        public IActionResult CheckEmployees()
        {
            var id = Convert.ToInt32(HttpContext.Session.GetString(SessionId));
            var empId = db.Employees.Where(x => x.EmployerId == id).Select(x => x.EmployeeId).ToList();
            List<UserProfile> employees = CreateEmployeesSetForBaessianCheck();
            var withoutCommand = CreateEmployeesWithoutCommantSetForBaessianCheck();

            ViewBag.Group = new List<int>();
            ViewBag.Free = new List<UserProfile>();
            var minAmount = employees.GroupBy(x => x.CommandId).Min(g => g.Count());
            var maxAmount = employees.GroupBy(x => x.CommandId).Min(g => g.Count());
            List<int> commands = CreateSetOfCommands().Select(x => x.Id).ToList();
            List<string> warnings = new List<string>();
            List<string> groupsName = new List<string>();
            List<int> groups = new List<int>();
            List<int> groupsTree = new List<int>();
            List<int> notSignificant = new List<int>();
            List<int> ids = new List<int>();
            int uncorrectBayessian = 0, uncorrectTree = 0;
            List<string> checkCommands = CheckCommands(withoutCommand, employees, CreateSetOfCommands(),out ids);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            if (employees.Count() != 0 && withoutCommand.Count() != 0 && minAmount > 1 && maxAmount > 1 && commands.Count() > 1)
            {
                var employee = employees[employees.Count() - 1];
                foreach (var c in commands)
                {
                    var ammount = employees.Where(x => x.CommandId == c).ToList().Count();
                    if (ammount <= 1)
                    {
                        commands.RemoveAt(c);
                    }
                }
                BayesClassifier classifier = new BayesClassifier(employees, withoutCommand, commands);
                classifier.FindCommands();
                groups = classifier.FindPosteriorNumerator();
                notSignificant = classifier.NotSignificantColumns;
            }
            stopwatch.Stop();
            ViewBag.Time = stopwatch.ElapsedMilliseconds;
            ViewBag.Efficiency = FillEfficiencyCommand();
            ViewBag.EfficiencyB = FillEfficiencyCommandBaessian();
            ViewBag.EfficiencyT = FillEfficiencyCommandTree();
            //checkCommands = Change(groups, ids, CreateSetOfCommands(), out uncorrect);
            ViewBag.Group = groups;
            ViewBag.NotSignificant = notSignificant;
            List<string> cNames = new List<string>();
            cNames = CreateSetOfCommands().Select(x => x.Name).ToList();
            ViewBag.Commands = cNames;
            ViewBag.Free = withoutCommand;
            ViewBag.CheckCommands = checkCommands;
            FactorCorrelation factorCorrelation = new FactorCorrelation(employees);
            var order = factorCorrelation.OrderOfFactors();
            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();
            groupsTree = TreeOfDecision(withoutCommand, order, commands, employees);
            stopwatch1.Stop();
            ViewBag.Time1 = stopwatch1.ElapsedMilliseconds;
            ViewBag.GroupTree = groupsTree;
            List<string> names = new List<string>();
            List<TypeStatistics> statisticsB = new List<TypeStatistics>();
            List<TypeStatistics> statisticsT = new List<TypeStatistics>();
            for (int i = 0; i < cNames.Count; i++)
            {
                int total = withoutCommand.Where(x => x.CommandId == commands[i]).Count();
                int uncorrect = withoutCommand.Where(x => x.CommandId == commands[i] && x.CommandId != groups[x.Id - 1]).Count();
                int trueDetected = withoutCommand.Where(x => x.CommandId == commands[i] && x.CommandId == groups[x.Id - 1]).Count();
                statisticsB.Add(new TypeStatistics(cNames[i], (double)trueDetected/total, uncorrect, total));
             
                uncorrect = withoutCommand.Where(x => x.CommandId == commands[i] && x.CommandId != groupsTree[x.Id - 1]).Count();
                trueDetected = withoutCommand.Where(x => x.CommandId == commands[i] && x.CommandId == groupsTree[x.Id - 1]).Count();
                statisticsT.Add(new TypeStatistics(cNames[i], (double)trueDetected / total, uncorrect, total));
            }
            //for (int i = 0; i < cNames.Count; i++)
            //{
            //    names.Add(cNames[withoutCommand[i].CommandId - 1]);
            //}
            names = withoutCommand.Select(x => cNames[x.CommandId - 1]).ToList();
            ViewBag.CNames = names;
            ViewBag.StatisticsB = statisticsB;
            ViewBag.StatisticsT = statisticsT;
            if (withoutCommand.Count() == 0)
            {
                warnings.Add("There is no free employees!");
            }
            if (maxAmount <= 1)
            {
                warnings.Add("Commands without employees or with one employee are not taken for search!");
            }
            if (commands.Count() <= 1)
            {
                warnings.Add("There is very little commands with more than one employee!");
            }
            ViewBag.Warnings = warnings;
            ViewBag.UncorrectB = uncorrectBayessian;
            ViewBag.UncorrectT = uncorrectTree;
            ViewBag.AccuracyB = Math.Round(100 * (1 - ((double)uncorrectBayessian / withoutCommand.Count())), 3);
            ViewBag.AccuracyT = Math.Round(100 * (1 - ((double)uncorrectTree / withoutCommand.Count())), 3);
            return View(employees);
        }
        public List<int> TreeOfDecision(List<UserProfile> withoutCommand, int[] orderOfFactors, List<int> commands, List<UserProfile> employees)
        {
            List<int> commandsIds = new List<int>();
            double[,] averages = new double[5, commands.Count()];
            for (int i = 0; i < commands.Count(); i++)
            {
                int count = employees.Where(x => x.CommandId == commands[i]).Count();
                if (count != 0)
                {
                    averages[0, i] = (double)(employees.Where(x => x.CommandId == commands[i]).Sum(x => x.Level)) / count;
                    averages[1, i] = (double)(employees.Where(x => x.CommandId == commands[i]).Sum(x => x.AbilityToWorkInHome)) / count;
                    averages[2, i] = (double)(employees.Where(x => x.CommandId == commands[i]).Sum(x => x.AbilityToWorkInOffice)) / count;
                    averages[3, i] = (double)(employees.Where(x => x.CommandId == commands[i]).Sum(x => x.Experience)) / count;
                    averages[4, i] = (double)(employees.Where(x => x.CommandId == commands[i]).Sum(x => x.AverageComplexityOfTasks)) / count;
                }
            }
            for (int j = 0; j < withoutCommand.Count(); j++)
            {
                double[] userWithoutCommand = new double[] { withoutCommand[j].Level, withoutCommand[j].AbilityToWorkInHome, withoutCommand[j].AbilityToWorkInOffice,
                withoutCommand[j].Experience, withoutCommand[j].AverageComplexityOfTasks};
                List<int> currentCommands = new List<int>(commands);
                List<int> bestCommands = new List<int>();
                int commandId = 0;
                for (int i = 0; i < orderOfFactors.Length; i++)
                {
                    double minAvg = 1000;
                    bool isSame = false;
                    int commandID = 0;
                    for (int k = 0; k < currentCommands.Count(); k++)
                    {
                        if (minAvg > Math.Abs(averages[orderOfFactors[i], k] - userWithoutCommand[orderOfFactors[i]]))
                        {
                            minAvg = Math.Abs(averages[orderOfFactors[i], k] - userWithoutCommand[orderOfFactors[i]]);
                            commandID = currentCommands[k];
                        }
                    }
                    bestCommands.Add(commandID);
                }
                int[,] counter = new int[2, commands.Count()];
                for (int t = 0; t < commands.Count(); t++)
                {
                    counter[0, t] = commands[t];
                    counter[1, t] = bestCommands.Where(x => x == commands[t]).Count();
                }
                int max = -100;
                int cId = 0;
                for (int t = 0; t < commands.Count(); t++)
                {
                    if (max < counter[1, t])
                    {
                        max = counter[1, t];
                        cId = counter[0, t];
                    }
                    else if (max == counter[1, t])
                    {
                        int first = bestCommands.IndexOf(cId);
                        int second = bestCommands.IndexOf(counter[0, t]);
                        if (second < first)
                        {
                            cId = counter[0, t];
                        }
                    }
                }
                commandsIds.Add(cId);
            }
            return commandsIds;
        }
        public List<string> CheckCommands(List<UserProfile> withoutCommands, List<UserProfile> employees, List<Command> commands, out List<int> ids)
        {
            List<string> commandsIds = new List<string>();
            ids = new List<int>();
            double[,] avgs = new double[commands.Count(), 5];
            double[,] deviations = new double[withoutCommands.Count(), commands.Count()];
            foreach (var e in employees)
            {
                avgs[e.CommandId - 1, 0] += e.Level;
                avgs[e.CommandId - 1, 1] += e.AbilityToWorkInHome;
                avgs[e.CommandId - 1, 2] += e.AbilityToWorkInOffice;
                avgs[e.CommandId - 1, 3] += e.AverageComplexityOfTasks;
                avgs[e.CommandId - 1, 4] += e.Experience;
            }
            for (int i = 0; i < commands.Count(); i++)
            {
                var count = employees.Where(x => x.CommandId == i + 1).Count();
                for (int j = 0; j < 5; j++)
                {
                    avgs[i, j] /= count;
                }
            }
            for (int i = 0; i < withoutCommands.Count(); i++)
            {
                for (int j = 0; j < commands.Count(); j++)
                {
                    deviations[i, j] += Math.Abs(avgs[j, 0] - withoutCommands[i].Level);
                    deviations[i, j] += Math.Abs(avgs[j, 1] - withoutCommands[i].AbilityToWorkInHome);
                    deviations[i, j] += Math.Abs(avgs[j, 2] - withoutCommands[i].AbilityToWorkInOffice);
                    deviations[i, j] += Math.Abs(avgs[j, 3] - withoutCommands[i].AverageComplexityOfTasks);
                    deviations[i, j] += Math.Abs(avgs[j, 4] - withoutCommands[i].Experience);
                }
            }
            for (int i = 0; i < withoutCommands.Count(); i++)
            {
                string command = "";
                double min = 1000;
                for (int j = 0; j < commands.Count(); j++)
                {
                    if (deviations[i, j] < min)
                    {
                        min = deviations[i, j];
                        command = commands.Where(x => x.Id == j+1).Select(x => x.Name).FirstOrDefault();
                        ids.Add(j + 1);
                    }
                }
                commandsIds.Add(command);
            }
            return commandsIds;
        }
        public List<UserSkillModel> CreateSetForCMeansCheck()
        {
            List<UserSkillModel> usersSet = new List<UserSkillModel>();
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user1", SkillEvaluations = new int[] { 5, 5, 0, 1, 2, 0, 2, 0, 1, 0, 0, 0 }, Position = "Android devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user2", SkillEvaluations = new int[] { 1, 2, 0, 0, 1, 4, 0, 4, 1, 0, 5, 0 }, Position = "Back-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user3", SkillEvaluations = new int[] { 1, 0, 2, 4, 5, 0, 1, 1, 1, 0, 2, 0 }, Position = "Game devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user4", SkillEvaluations = new int[] { 1, 0, 2, 0, 1, 0, 5, 1, 1, 0, 0, 4 }, Position = "iOS devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user5", SkillEvaluations = new int[] { 0, 2, 0, 1, 1, 0, 2, 0, 4, 5, 0, 0 }, Position = "Front-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user6", SkillEvaluations = new int[] { 3, 3, 0, 1, 2, 0, 2, 0, 0, 0, 1, 2 }, Position = "Android devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user7", SkillEvaluations = new int[] { 1, 2, 1, 1, 1, 4, 0, 2, 1, 0, 3, 0 }, Position = "Back-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user8", SkillEvaluations = new int[] { 1, 2, 2, 2, 3, 0, 1, 1, 1, 0, 0, 2 }, Position = "Game devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user9", SkillEvaluations = new int[] { 1, 0, 2, 1, 1, 0, 3, 1, 1, 0, 2, 5 }, Position = "iOS devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user10", SkillEvaluations = new int[] { 1, 1, 2, 1, 1, 0, 2, 0, 4, 3, 2, 2 }, Position = "Front-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user11", SkillEvaluations = new int[] { 3, 4, 1, 1, 1, 2, 2, 1, 1, 1, 1, 2 }, Position = "Android devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user12", SkillEvaluations = new int[] { 1, 2, 1, 1, 1, 4, 1, 3, 1, 1, 4, 1 }, Position = "Back-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user13", SkillEvaluations = new int[] { 1, 2, 5, 3, 3, 1, 1, 1, 1, 1, 1, 2 }, Position = "Game devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user14", SkillEvaluations = new int[] { 1, 1, 2, 1, 1, 1, 3, 1, 1, 1, 1, 5 }, Position = "iOS devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user15", SkillEvaluations = new int[] { 1, 1, 2, 1, 1, 1, 1, 1, 3, 3, 2, 2 }, Position = "Front-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user16", SkillEvaluations = new int[] { 5, 4, 2, 2, 2, 2, 2, 3, 2, 2, 1, 2 }, Position = "Android devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user17", SkillEvaluations = new int[] { 2, 2, 2, 2, 2, 5, 2, 4, 2, 2, 3, 2 }, Position = "Back-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user18", SkillEvaluations = new int[] { 2, 2, 5, 3, 4, 2, 2, 2, 2, 2, 1, 2 }, Position = "Game devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user19", SkillEvaluations = new int[] { 2, 2, 2, 2, 2, 1, 5, 2, 2, 2, 2, 5 }, Position = "iOS devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user20", SkillEvaluations = new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 5, 4, 2, 2 }, Position = "Front-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user21", SkillEvaluations = new int[] { 5, 4, 1, 2, 1, 2, 0, 1, 1, 1, 0, 3 }, Position = "Android devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user22", SkillEvaluations = new int[] { 1, 0, 3, 1, 0, 4, 1, 5, 1, 1, 3, 3 }, Position = "Back-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user23", SkillEvaluations = new int[] { 1, 0, 5, 3, 5, 1, 3, 1, 0, 1, 3, 2 }, Position = "Game devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user24", SkillEvaluations = new int[] { 0, 4, 2, 1, 0, 1, 3, 1, 3, 1, 1, 5 }, Position = "iOS devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user25", SkillEvaluations = new int[] { 4, 0, 2, 0, 3, 1, 0, 1, 5, 4, 0, 2 }, Position = "Front-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user26", SkillEvaluations = new int[] { 5, 4, 0, 2, 4, 0, 0, 4, 2, 0, 1, 1 }, Position = "Android devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user27", SkillEvaluations = new int[] { 3, 1, 0, 1, 3, 5, 0, 4, 1, 2, 4, 2 }, Position = "Back-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user28", SkillEvaluations = new int[] { 4, 0, 5, 3, 4, 1, 4, 0, 2, 0, 3, 2 }, Position = "Game devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user29", SkillEvaluations = new int[] { 3, 1, 0, 4, 0, 1, 5, 2, 0, 3, 2, 5 }, Position = "iOS devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user30", SkillEvaluations = new int[] { 3, 1, 0, 2, 0, 2, 1, 2, 5, 4, 0, 4 }, Position = "Front-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user31", SkillEvaluations = new int[] { 5, 4, 0, 3, 1, 0, 4, 0, 2, 1, 2, 3 }, Position = "Android devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user32", SkillEvaluations = new int[] { 1, 2, 3, 0, 3, 5, 1, 4, 1, 2, 2, 3 }, Position = "Back-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user33", SkillEvaluations = new int[] { 1, 0, 5, 3, 5, 1, 3, 1, 0, 1, 3, 2 }, Position = "Game devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user34", SkillEvaluations = new int[] { 2, 4, 2, 2, 0, 2, 4, 2, 3, 2, 2, 5 }, Position = "iOS devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user35", SkillEvaluations = new int[] { 4, 0, 2, 3, 3, 1, 3, 1, 5, 5, 3, 2 }, Position = "Front-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user36", SkillEvaluations = new int[] { 5, 4, 1, 2, 4, 1, 1, 4, 2, 1, 3, 3 }, Position = "Android devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user37", SkillEvaluations = new int[] { 3, 1, 1, 1, 3, 5, 1, 4, 1, 2, 5, 2 }, Position = "Back-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user28", SkillEvaluations = new int[] { 4, 1, 5, 3, 4, 1, 4, 2, 2, 2, 3, 2 }, Position = "Game devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user39", SkillEvaluations = new int[] { 2, 1, 2, 4, 2, 2, 5, 2, 2, 3, 2, 4 }, Position = "iOS devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user40", SkillEvaluations = new int[] { 3, 1, 1, 2, 1, 2, 1, 2, 4, 4, 1, 4 }, Position = "Front-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user41", SkillEvaluations = new int[] { 5, 5, 3, 3, 1, 1, 3, 3, 1, 1, 3, 3 }, Position = "Android devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user42", SkillEvaluations = new int[] { 1, 2, 3, 1, 2, 4, 3, 4, 1, 2, 4, 3 }, Position = "Back-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user43", SkillEvaluations = new int[] { 1, 3, 4, 3, 4, 1, 3, 1, 3, 1, 3, 3 }, Position = "Game devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user44", SkillEvaluations = new int[] { 2, 4, 2, 3, 2, 2, 4, 2, 1, 2, 1, 5 }, Position = "iOS devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user45", SkillEvaluations = new int[] { 3, 2, 3, 2, 3, 2, 3, 2, 5, 5, 3, 2 }, Position = "Front-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user46", SkillEvaluations = new int[] { 5, 4, 1, 1, 2, 1, 1, 2, 2, 1, 3, 3 }, Position = "Android devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user47", SkillEvaluations = new int[] { 3, 1, 3, 1, 3, 5, 1, 4, 3, 1, 5, 3 }, Position = "Back-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user48", SkillEvaluations = new int[] { 3, 1, 5, 3, 4, 3, 1, 3, 1, 2, 3, 2 }, Position = "Game devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user49", SkillEvaluations = new int[] { 2, 1, 2, 1, 2, 2, 4, 2, 2, 3, 2, 4 }, Position = "iOS devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user50", SkillEvaluations = new int[] { 3, 1, 2, 2, 2, 2, 1, 2, 4, 4, 1, 4 }, Position = "Front-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user51", SkillEvaluations = new int[] { 5, 5, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, Position = "Android devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user52", SkillEvaluations = new int[] { 3, 3, 3, 1, 2, 5, 0, 5, 1, 2, 5, 3 }, Position = "Back-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user53", SkillEvaluations = new int[] { 1, 3, 4, 5, 4, 2, 3, 2, 3, 1, 0, 0 }, Position = "Game devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user54", SkillEvaluations = new int[] { 2, 3, 2, 3, 2, 2, 5, 2, 3, 2, 3, 5 }, Position = "iOS devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user55", SkillEvaluations = new int[] { 4, 2, 3, 2, 1, 2, 3, 2, 5, 5, 4, 2 }, Position = "Front-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user56", SkillEvaluations = new int[] { 3, 3, 1, 1, 1, 2, 2, 1, 1, 1, 1, 3 }, Position = "Android devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user57", SkillEvaluations = new int[] { 3, 1, 3, 1, 3, 5, 1, 4, 3, 1, 5, 3 }, Position = "Back-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user58", SkillEvaluations = new int[] { 3, 1, 5, 5, 5, 3, 1, 0, 1, 2, 0, 2 }, Position = "Game devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user59", SkillEvaluations = new int[] { 2, 1, 4, 1, 0, 0, 4, 2, 4, 0, 2, 5 }, Position = "iOS devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user60", SkillEvaluations = new int[] { 3, 2, 2, 2, 2, 0, 2, 2, 5, 5, 2, 4 }, Position = "Front-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user61", SkillEvaluations = new int[] { 5, 5, 3, 0, 3, 1, 0, 3, 3, 0, 3, 3 }, Position = "Android devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user62", SkillEvaluations = new int[] { 1, 2, 4, 1, 1, 5, 0, 5, 1, 2, 5, 1 }, Position = "Back-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user63", SkillEvaluations = new int[] { 2, 3, 4, 5, 4, 2, 1, 2, 0, 1, 0, 1 }, Position = "Game devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user64", SkillEvaluations = new int[] { 2, 1, 3, 3, 1, 2, 5, 2, 3, 2, 3, 5 }, Position = "iOS devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user65", SkillEvaluations = new int[] { 2, 2, 0, 2, 1, 2, 3, 2, 5, 3, 4, 0 }, Position = "Front-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user66", SkillEvaluations = new int[] { 3, 4, 1, 2, 1, 2, 2, 2, 1, 2, 1, 3 }, Position = "Android devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user67", SkillEvaluations = new int[] { 1, 1, 2, 1, 3, 5, 1, 4, 0, 1, 5, 0 }, Position = "Back-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user68", SkillEvaluations = new int[] { 2, 1, 5, 4, 3, 0, 1, 0, 1, 2, 3, 2 }, Position = "Game devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user69", SkillEvaluations = new int[] { 2, 1, 3, 1, 0, 0, 4, 2, 3, 0, 2, 5 }, Position = "iOS devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user70", SkillEvaluations = new int[] { 3, 2, 1, 2, 1, 0, 2, 2, 3, 5, 2, 3 }, Position = "Front-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user71", SkillEvaluations = new int[] { 3, 2, 2, 2, 2, 0, 2, 2, 5, 5, 2, 4 }, Position = "Front-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user72", SkillEvaluations = new int[] { 5, 5, 3, 0, 3, 1, 0, 3, 3, 0, 3, 3 }, Position = "Android devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user73", SkillEvaluations = new int[] { 1, 2, 4, 1, 1, 5, 0, 5, 1, 2, 5, 1 }, Position = "Back-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user74", SkillEvaluations = new int[] { 2, 3, 4, 5, 4, 2, 1, 2, 0, 1, 0, 1 }, Position = "Game devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user75", SkillEvaluations = new int[] { 2, 1, 3, 3, 1, 2, 5, 2, 3, 2, 3, 5 }, Position = "iOS devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user76", SkillEvaluations = new int[] { 2, 2, 0, 2, 1, 2, 3, 2, 5, 3, 4, 0 }, Position = "Front-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user77", SkillEvaluations = new int[] { 3, 4, 1, 2, 1, 2, 2, 2, 1, 2, 1, 3 }, Position = "Android devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user78", SkillEvaluations = new int[] { 1, 1, 2, 1, 3, 5, 1, 4, 0, 1, 5, 0 }, Position = "Back-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user79", SkillEvaluations = new int[] { 2, 1, 5, 4, 3, 0, 1, 0, 1, 2, 3, 2 }, Position = "Game devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user80", SkillEvaluations = new int[] { 2, 1, 3, 1, 0, 0, 4, 2, 3, 0, 2, 5 }, Position = "iOS devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user81", SkillEvaluations = new int[] { 3, 2, 1, 2, 1, 0, 2, 2, 3, 5, 2, 3 }, Position = "Front-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user82", SkillEvaluations = new int[] { 3, 3, 3, 1, 2, 5, 0, 5, 1, 2, 5, 3 }, Position = "Back-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user83", SkillEvaluations = new int[] { 1, 3, 4, 5, 4, 2, 3, 2, 3, 1, 0, 0 }, Position = "Game devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user84", SkillEvaluations = new int[] { 2, 3, 2, 3, 2, 2, 5, 2, 3, 2, 3, 5 }, Position = "iOS devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user85", SkillEvaluations = new int[] { 4, 2, 3, 2, 1, 2, 3, 2, 5, 5, 4, 2 }, Position = "Front-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user86", SkillEvaluations = new int[] { 3, 3, 1, 1, 1, 2, 2, 1, 1, 1, 1, 3 }, Position = "Android devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user87", SkillEvaluations = new int[] { 3, 1, 3, 1, 3, 5, 1, 4, 3, 1, 5, 3 }, Position = "Back-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user88", SkillEvaluations = new int[] { 3, 1, 5, 5, 5, 3, 1, 0, 1, 2, 0, 2 }, Position = "Game devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user89", SkillEvaluations = new int[] { 2, 1, 4, 1, 0, 0, 4, 2, 4, 0, 2, 5 }, Position = "iOS devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user90", SkillEvaluations = new int[] { 3, 2, 2, 2, 2, 0, 2, 2, 5, 5, 2, 4 }, Position = "Front-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user91", SkillEvaluations = new int[] { 5, 5, 3, 3, 1, 1, 3, 3, 1, 1, 3, 3 }, Position = "Android devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user92", SkillEvaluations = new int[] { 1, 2, 3, 1, 2, 4, 3, 4, 1, 2, 4, 3 }, Position = "Back-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user93", SkillEvaluations = new int[] { 1, 3, 4, 3, 4, 1, 3, 1, 3, 1, 3, 3 }, Position = "Game devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user94", SkillEvaluations = new int[] { 2, 4, 2, 3, 2, 2, 4, 2, 1, 2, 1, 5 }, Position = "iOS devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user95", SkillEvaluations = new int[] { 3, 2, 3, 2, 3, 2, 3, 2, 5, 5, 3, 2 }, Position = "Front-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user96", SkillEvaluations = new int[] { 5, 4, 1, 1, 2, 1, 1, 2, 2, 1, 3, 3 }, Position = "Android devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user97", SkillEvaluations = new int[] { 3, 1, 3, 1, 3, 5, 1, 4, 3, 1, 5, 3 }, Position = "Back-end devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user98", SkillEvaluations = new int[] { 3, 1, 5, 3, 4, 3, 1, 3, 1, 2, 3, 2 }, Position = "Game devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user99", SkillEvaluations = new int[] { 2, 1, 2, 1, 2, 2, 4, 2, 2, 3, 2, 4 }, Position = "iOS devs" });
            usersSet.Add(new UserSkillModel(db.Groups.Count()) { FirstName = "user100", SkillEvaluations = new int[] { 3, 1, 2, 2, 2, 2, 1, 2, 4, 4, 1, 4 }, Position = "Front-end devs" });
            return usersSet;
        }

        public List<UserProfile> CreateEmployeesSetForBaessianCheck()
        {
            List<UserProfile> employees = new List<UserProfile>();
            employees.Add(new UserProfile() { Id = 2, CommandId = 1, Login = "user1", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 1, Experience = 3 });
            employees.Add(new UserProfile() { Id = 3, CommandId = 1, Login = "user2", Level = 1, AbilityToWorkInHome = 0, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 5 });
            employees.Add(new UserProfile() { Id = 4, CommandId = 1, Login = "user3", Level = 2, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 5 });
            employees.Add(new UserProfile() { Id = 5, CommandId = 1, Login = "user4", Level = 2, AbilityToWorkInHome = 0, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 3, Experience = 5 });
            employees.Add(new UserProfile() { Id = 6, CommandId = 1, Login = "user5", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 3, Experience = 3 });
            employees.Add(new UserProfile() { Id = 7, CommandId = 2, Login = "user6", Level = 2, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 2, Experience = 13 });
            employees.Add(new UserProfile() { Id = 8, CommandId = 2, Login = "user7", Level = 2, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 2, Experience = 15 });
            employees.Add(new UserProfile() { Id = 9, CommandId = 2, Login = "user8", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 3, Experience = 17 });
            employees.Add(new UserProfile() { Id = 10, CommandId = 2, Login = "user9", Level = 3, AbilityToWorkInHome = 0, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 3, Experience = 15 });
            employees.Add(new UserProfile() { Id = 11, CommandId = 2, Login = "user10", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 3, Experience = 13 });
            employees.Add(new UserProfile() { Id = 12, CommandId = 3, Login = "user11", Level = 1, AbilityToWorkInHome = 0, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 1, Experience = 1 });
            employees.Add(new UserProfile() { Id = 13, CommandId = 3, Login = "user12", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 1, Experience = 2 });
            employees.Add(new UserProfile() { Id = 14, CommandId = 3, Login = "user13", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 1, Experience = 2 });
            employees.Add(new UserProfile() { Id = 15, CommandId = 3, Login = "user14", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 2 });
            employees.Add(new UserProfile() { Id = 16, CommandId = 3, Login = "user15", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 3, Experience = 5 });
            return employees;
        }
        public List<Command> CreateSetOfCommands()
        {
            List<Command> commands = new List<Command>();
            commands.Add( new Command() { Id = 1, Name = "command1", EmployerId = 1, Description = "product1" });
            commands.Add( new Command() { Id = 2, Name = "command2", EmployerId = 1, Description = "product2" });
            commands.Add( new Command() { Id = 3, Name = "command3", EmployerId = 1, Description = "product3" });

            return commands;
        }

        public List<UserProfile> CreateEmployeesWithoutCommantSetForBaessianCheck()
        {
            List<UserProfile> employees = new List<UserProfile>();
            employees.Add(new UserProfile() { Id = 1, CommandId = 3, Login = "user116", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 1, Experience = 1 });
            employees.Add(new UserProfile() { Id = 2, CommandId = 3, Login = "user16", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 1, Experience = 1 });
            employees.Add(new UserProfile() { Id = 3, CommandId = 3, Login = "user17", Level = 1, AbilityToWorkInHome = 0, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 1, Experience = 2 });
            employees.Add(new UserProfile() { Id = 4, CommandId = 1, Login = "user18", Level = 2, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 2, Experience = 7 });
            employees.Add(new UserProfile() { Id = 5, CommandId = 1, Login = "user19", Level = 2, AbilityToWorkInHome = 0, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 5 });
            employees.Add(new UserProfile() { Id = 6, CommandId = 2, Login = "user20", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 3, Experience = 13 });
            employees.Add(new UserProfile() { Id = 7, CommandId = 2, Login = "user21", Level = 2, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 3, Experience = 3 });
            employees.Add(new UserProfile() { Id = 8, CommandId = 1, Login = "user22", Level = 2, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 5 });
            employees.Add(new UserProfile() { Id = 9, CommandId = 2, Login = "user23", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 3, Experience = 14 });
            employees.Add(new UserProfile() { Id = 10, CommandId = 2, Login = "user24", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 3, Experience = 10 });
            employees.Add(new UserProfile() { Id = 11, CommandId = 2, Login = "user25", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 3, Experience = 13 });
            employees.Add(new UserProfile() { Id = 12, CommandId = 3, Login = "user26", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 1, Experience = 1 });
            employees.Add(new UserProfile() { Id = 13, CommandId = 1, Login = "user27", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 2 });
            employees.Add(new UserProfile() { Id = 14, CommandId = 3, Login = "user28", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 1, Experience = 2 });
            employees.Add(new UserProfile() { Id = 15, CommandId = 1, Login = "user29", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 2 });
            employees.Add(new UserProfile() { Id = 16, CommandId = 2, Login = "user30", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 3, Experience = 9 });
            employees.Add(new UserProfile() { Id = 17, CommandId = 1, Login = "user31", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 3, Experience = 13 });
            employees.Add(new UserProfile() { Id = 18, CommandId = 1, Login = "user32", Level = 2, AbilityToWorkInHome = 0, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 1 });
            employees.Add(new UserProfile() { Id = 19, CommandId = 3, Login = "user33", Level = 1, AbilityToWorkInHome = 0, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 1, Experience = 2 });
            employees.Add(new UserProfile() { Id = 20, CommandId = 2, Login = "user34", Level = 2, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 3, Experience = 2 });
            employees.Add(new UserProfile() { Id = 21, CommandId = 1, Login = "user35", Level = 2, AbilityToWorkInHome = 0, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 2 });
            employees.Add(new UserProfile() { Id = 22, CommandId = 2, Login = "user36", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 3, Experience = 9 });
            employees.Add(new UserProfile() { Id = 23, CommandId = 2, Login = "user37", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 3, Experience = 8 });
            employees.Add(new UserProfile() { Id = 24, CommandId = 2, Login = "user38", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 3, Experience = 10 });
            employees.Add(new UserProfile() { Id = 25, CommandId = 1, Login = "user39", Level = 2, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 3, Experience = 4 });
            employees.Add(new UserProfile() { Id = 26, CommandId = 3, Login = "user40", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 1, Experience = 3 });
            employees.Add(new UserProfile() { Id = 27, CommandId = 2, Login = "user41", Level = 2, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 1, Experience = 1 });
            employees.Add(new UserProfile() { Id = 28, CommandId = 1, Login = "user42", Level = 2, AbilityToWorkInHome = 0, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 5 });
            employees.Add(new UserProfile() { Id = 29, CommandId = 2, Login = "user43", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 3, Experience = 13 });
            employees.Add(new UserProfile() { Id = 30, CommandId = 2, Login = "user44", Level = 2, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 3, Experience = 10 });
            employees.Add(new UserProfile() { Id = 31, CommandId = 2, Login = "user45", Level = 3, AbilityToWorkInHome = 0, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 3, Experience = 13 });
            employees.Add(new UserProfile() { Id = 32, CommandId = 1, Login = "user46", Level = 2, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 1, Experience = 4 });
            employees.Add(new UserProfile() { Id = 33, CommandId = 2, Login = "user47", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 3, Experience = 5 });
            employees.Add(new UserProfile() { Id = 34, CommandId = 2, Login = "user48", Level = 2, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 3, Experience = 2 });
            employees.Add(new UserProfile() { Id = 35, CommandId = 1, Login = "user49", Level = 1, AbilityToWorkInHome = 0, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 7 });
            employees.Add(new UserProfile() { Id = 36, CommandId = 2, Login = "user50", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 3, Experience = 9 });
            employees.Add(new UserProfile() { Id = 37, CommandId = 3, Login = "user51", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 1, Experience = 1 });
            employees.Add(new UserProfile() { Id = 38, CommandId = 3, Login = "user52", Level = 1, AbilityToWorkInHome = 0, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 1, Experience = 2 });
            employees.Add(new UserProfile() { Id = 39, CommandId = 1, Login = "user53", Level = 2, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 2, Experience = 7 });
            employees.Add(new UserProfile() { Id = 40, CommandId = 1, Login = "user54", Level = 2, AbilityToWorkInHome = 0, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 5 });
            employees.Add(new UserProfile() { Id = 41, CommandId = 2, Login = "user55", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 3, Experience = 13 });
            employees.Add(new UserProfile() { Id = 42, CommandId = 2, Login = "user56", Level = 2, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 3, Experience = 3 });
            employees.Add(new UserProfile() { Id = 43, CommandId = 1, Login = "user57", Level = 2, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 5 });
            employees.Add(new UserProfile() { Id = 44, CommandId = 2, Login = "user58", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 3, Experience = 14 });
            employees.Add(new UserProfile() { Id = 45, CommandId = 2, Login = "user59", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 3, Experience = 10 });
            employees.Add(new UserProfile() { Id = 46, CommandId = 2, Login = "user60", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 3, Experience = 13 });
            employees.Add(new UserProfile() { Id = 47, CommandId = 3, Login = "user61", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 1, Experience = 1 });
            employees.Add(new UserProfile() { Id = 48, CommandId = 1, Login = "user62", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 2 });
            employees.Add(new UserProfile() { Id = 49, CommandId = 3, Login = "user63", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 1, Experience = 2 });
            employees.Add(new UserProfile() { Id = 50, CommandId = 1, Login = "user64", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 2 });
            employees.Add(new UserProfile() { Id = 51, CommandId = 2, Login = "user65", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 3, Experience = 9 });
            employees.Add(new UserProfile() { Id = 52, CommandId = 2, Login = "user67", Level = 1, AbilityToWorkInHome = 0, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 1, Experience = 2 });
            employees.Add(new UserProfile() { Id = 53, CommandId = 1, Login = "user68", Level = 2, AbilityToWorkInHome = 0, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 1 });
            employees.Add(new UserProfile() { Id = 54, CommandId = 2, Login = "user69", Level = 3, AbilityToWorkInHome = 0, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 3, Experience = 13 });
            employees.Add(new UserProfile() { Id = 55, CommandId = 1, Login = "user70", Level = 2, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 1, Experience = 4 });
            employees.Add(new UserProfile() { Id = 56, CommandId = 2, Login = "user71", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 3, Experience = 5 });
            employees.Add(new UserProfile() { Id = 57, CommandId = 2, Login = "user72", Level = 2, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 3, Experience = 2 });
            employees.Add(new UserProfile() { Id = 58, CommandId = 1, Login = "user73", Level = 1, AbilityToWorkInHome = 0, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 7 });
            employees.Add(new UserProfile() { Id = 59, CommandId = 2, Login = "user74", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 3, Experience = 9 });
            employees.Add(new UserProfile() { Id = 60, CommandId = 3, Login = "user75", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 1, Experience = 1 });
            employees.Add(new UserProfile() { Id = 61, CommandId = 3, Login = "user76", Level = 1, AbilityToWorkInHome = 0, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 1, Experience = 2 });
            employees.Add(new UserProfile() { Id = 62, CommandId = 1, Login = "user77", Level = 2, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 2, Experience = 7 });
            employees.Add(new UserProfile() { Id = 63, CommandId = 1, Login = "user78", Level = 2, AbilityToWorkInHome = 0, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 5 });
            employees.Add(new UserProfile() { Id = 64, CommandId = 2, Login = "user79", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 3, Experience = 13 });
            employees.Add(new UserProfile() { Id = 65, CommandId = 2, Login = "user80", Level = 2, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 3, Experience = 3 });
            employees.Add(new UserProfile() { Id = 66, CommandId = 1, Login = "user81", Level = 2, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 5 });
            employees.Add(new UserProfile() { Id = 67, CommandId = 2, Login = "user82", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 3, Experience = 14 });
            employees.Add(new UserProfile() { Id = 68, CommandId = 2, Login = "user83", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 3, Experience = 10 });
            employees.Add(new UserProfile() { Id = 69, CommandId = 2, Login = "user84", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 3, Experience = 13 });
            employees.Add(new UserProfile() { Id = 70, CommandId = 2, Login = "user85", Level = 2, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 3, Experience = 3 });
            employees.Add(new UserProfile() { Id = 71, CommandId = 1, Login = "user86", Level = 2, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 5 });
            employees.Add(new UserProfile() { Id = 72, CommandId = 2, Login = "user87", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 3, Experience = 14 });
            employees.Add(new UserProfile() { Id = 73, CommandId = 2, Login = "user88", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 3, Experience = 10 });
            employees.Add(new UserProfile() { Id = 74, CommandId = 2, Login = "user89", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 3, Experience = 13 });
            employees.Add(new UserProfile() { Id = 75, CommandId = 3, Login = "user90", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 1, Experience = 1 });
            employees.Add(new UserProfile() { Id = 76, CommandId = 1, Login = "user91", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 2 });
            employees.Add(new UserProfile() { Id = 77, CommandId = 3, Login = "user92", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 1, Experience = 2 });
            employees.Add(new UserProfile() { Id = 78, CommandId = 1, Login = "user93", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 2 });
            employees.Add(new UserProfile() { Id = 79, CommandId = 3, Login = "user94", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 1, Experience = 1 });
            employees.Add(new UserProfile() { Id = 80, CommandId = 1, Login = "user95", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 2 });
            employees.Add(new UserProfile() { Id = 81, CommandId = 3, Login = "user96", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 1, Experience = 2 });
            employees.Add(new UserProfile() { Id = 82, CommandId = 1, Login = "user97", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 2 });
            employees.Add(new UserProfile() { Id = 83, CommandId = 2, Login = "user98", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 3, Experience = 9 });
            employees.Add(new UserProfile() { Id = 84, CommandId = 1, Login = "user99", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 3, Experience = 13 });
            employees.Add(new UserProfile() { Id = 85, CommandId = 1, Login = "user100", Level = 2, AbilityToWorkInHome = 0, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 1 });
            employees.Add(new UserProfile() { Id = 86, CommandId = 3, Login = "user101", Level = 1, AbilityToWorkInHome = 0, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 1, Experience = 2 });
            employees.Add(new UserProfile() { Id = 87, CommandId = 2, Login = "user102", Level = 2, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 3, Experience = 2 });
            employees.Add(new UserProfile() { Id = 88, CommandId = 1, Login = "user103", Level = 2, AbilityToWorkInHome = 0, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 2 });
            employees.Add(new UserProfile() { Id = 89, CommandId = 2, Login = "user104", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 3, Experience = 9 });
            employees.Add(new UserProfile() { Id = 90, CommandId = 2, Login = "user105", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 3, Experience = 8 });
            employees.Add(new UserProfile() { Id = 91, CommandId = 2, Login = "user106", Level = 3, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 3, Experience = 10 });
            employees.Add(new UserProfile() { Id = 92, CommandId = 1, Login = "user107", Level = 2, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 3, Experience = 4 });
            employees.Add(new UserProfile() { Id = 93, CommandId = 3, Login = "user108", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 1, Experience = 3 });
            employees.Add(new UserProfile() { Id = 94, CommandId = 2, Login = "user109", Level = 2, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 1, Experience = 1 });
            employees.Add(new UserProfile() { Id = 95, CommandId = 3, Login = "user110", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 1, Experience = 2 });
            employees.Add(new UserProfile() { Id = 96, CommandId = 1, Login = "user111", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 2 });
            employees.Add(new UserProfile() { Id = 97, CommandId = 3, Login = "user112", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 1, Experience = 1 });
            employees.Add(new UserProfile() { Id = 98, CommandId = 1, Login = "user113", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 2 });
            employees.Add(new UserProfile() { Id = 99, CommandId = 3, Login = "user114", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 0, AverageComplexityOfTasks = 1, Experience = 2 });
            employees.Add(new UserProfile() { Id = 100, CommandId = 1, Login = "user115", Level = 1, AbilityToWorkInHome = 1, AbilityToWorkInOffice = 1, AverageComplexityOfTasks = 2, Experience = 2 });
            
            return employees;
        }

        public List<Efficiency> FillEfficiencyCommand()
        {
            List<Efficiency> efficiency = new List<Efficiency>();
            efficiency.Add(new Efficiency(100, 6094.5, 1884.5));
            efficiency.Add(new Efficiency(200, 10849, 2446.25));
            efficiency.Add(new Efficiency(300, 15907.75, 4330.25));
            efficiency.Add(new Efficiency(400, 18505.5, 5339.5));
            efficiency.Add(new Efficiency(500, 24740, 5971.25));
            return efficiency;
        }

        public List<Efficiency> FillEfficiencyCommandBaessian()
        {
            List<Efficiency> efficiency = new List<Efficiency>();
            efficiency.Add(new Efficiency(100, 7124, 6360, 5376, 5516));
            efficiency.Add(new Efficiency(200, 10301, 15953, 9148, 7960));
            efficiency.Add(new Efficiency(300, 14269, 15287, 15196, 18709));
            efficiency.Add(new Efficiency(400, 21011, 17885, 18005, 17121));
            efficiency.Add(new Efficiency(500, 26898, 25870, 26305, 19887));
            return efficiency;
        }

        public List<Efficiency> FillEfficiencyCommandTree()
        {
            List<Efficiency> efficiency = new List<Efficiency>();
            efficiency.Add(new Efficiency(100, 1988, 2084, 1245, 2061));
            efficiency.Add(new Efficiency(200, 2261, 2148, 2277, 3099));
            efficiency.Add(new Efficiency(300, 4905, 4016, 4216, 4184));
            efficiency.Add(new Efficiency(400, 5043, 6999, 4259, 5057));
            efficiency.Add(new Efficiency(500, 7112, 6737, 5264, 4742));
            return efficiency;
        }

        public List<Efficiency> FillEfficiencyPosition()
        {
            List<Efficiency> efficiency = new List<Efficiency>();
            efficiency.Add(new Efficiency(100, 2624897, 1827489, 1836216, 1988633));
            efficiency.Add(new Efficiency(200, 4171482, 3119529, 3243570, 3350087));
            efficiency.Add(new Efficiency(300, 5669984, 4816400, 5105663, 5134992));
            efficiency.Add(new Efficiency(400, 7714925, 6448576, 6647241, 7206634));
            efficiency.Add(new Efficiency(500, 8482148, 8059157, 8960133, 8163811));
            return efficiency;
        }

        public List<Efficiency> FillEfficiencyForecast()
        {
            List<Efficiency> efficiency = new List<Efficiency>();
            efficiency.Add(new Efficiency(100, 5586.5, 913.25));
            efficiency.Add(new Efficiency(200, 11776.5, 1914.25));
            efficiency.Add(new Efficiency(300, 16761, 2441.25));
            efficiency.Add(new Efficiency(400, 22571.25, 3238.75));
            efficiency.Add(new Efficiency(500, 24907.5, 3921));
            return efficiency;
        }

        public List<Efficiency> FillEfficiencyArima()
        {
            List<Efficiency> efficiency = new List<Efficiency>();
            efficiency.Add(new Efficiency(100, 1033, 892, 952, 776));
            efficiency.Add(new Efficiency(200, 2059, 1783, 2008, 1807));
            efficiency.Add(new Efficiency(300, 2580, 2671, 2367, 2147));
            efficiency.Add(new Efficiency(400, 3538, 2932, 3199, 3286));
            efficiency.Add(new Efficiency(500, 3462, 3918, 4097, 4207));
            return efficiency;
        }

        public List<Efficiency> FillEfficiencyExponential()
        {
            List<Efficiency> efficiency = new List<Efficiency>();
            efficiency.Add(new Efficiency(100, 5863, 6297, 4613, 5573));
            efficiency.Add(new Efficiency(200, 11983, 11851, 12599, 10673));
            efficiency.Add(new Efficiency(300, 16953, 15687, 18041, 16363));
            efficiency.Add(new Efficiency(400, 21010, 25207, 24220, 19848));
            efficiency.Add(new Efficiency(500, 22776, 25900, 24567, 26387));
            return efficiency;
        }

        public List<string> Change(List<int> groups, List<int> ids, List<Command> commands, out int uncorrect)
        {
            List<string> names = new List<string>();
            List<int> rands = new List<int>();
            Random random = new Random();
            uncorrect = 0;
            for (int i = 0; i < 5; i++)
            {
                rands.Add(random.Next(0, groups.Count()));
            }
            for (int i = 0; i < groups.Count(); i++)
            {
                if (rands.Contains(i))
                {
                    names.Add(commands.Where(k => k.Id == ids[i]).Select(k => k.Name).FirstOrDefault());
                    if (commands.Where(k => k.Id == ids[i]).Select(k => k.Name).FirstOrDefault() != commands.Where(k => k.Id == groups[i]).Select(k => k.Name).FirstOrDefault())
                    {
                        uncorrect++;
                    }
                }
                else 
                {
                    names.Add(commands.Where(k => k.Id == groups[i]).Select(k => k.Name).FirstOrDefault());
                }
            }
            return names;
        }
    }
}
