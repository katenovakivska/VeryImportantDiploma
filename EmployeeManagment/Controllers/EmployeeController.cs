using EmployeeManagment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EmployeeManagment.Methods;
using Microsoft.AspNetCore.Http;

namespace EmployeeManagment.Controllers
{
    public class EmployeeController : Controller
    {
        EmployeeContext db;
        const string SessionName = "_Name";
        const string SessionId = "_Id";
        List<UserProfile> Employees { get; set; }
        public EmployeeController(EmployeeContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserProfile user)
        {

            if (user.FirstName != null || user.SecondName != null)
            {
                db.UserProfiles.Add(user);
                db.SaveChanges();
            }

            return View(user);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                return RedirectToAction("Login", "Authentication");
            }
            return View();
        }
        [HttpGet]
        public IActionResult CreateCommand()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AllCommands()
        {
            var commands = db.Commands.Where(x => x.EmployerId == Convert.ToInt32(HttpContext.Session.GetString(SessionId)));

            foreach (var c in commands)
            {
                var team = db.UserCommands.Where(x => x.CommandId == c.Id).ToList();
                c.UserCommands = new List<UserCommand>();
                foreach (var u in team)
                {
                    c.UserCommands.Add(u);
                    c.UserCommands.Last().User = db.UserProfiles.Where(x => x.Id == u.UserId).FirstOrDefault();
                }
            }
            var id = Convert.ToInt32(HttpContext.Session.GetString(SessionId));
            var empId = db.Employees.Where(x => x.EmployerId == id).Select(x => x.EmployeeId).ToList();
            var withoutCommand = db.UserProfiles.Where(x => x.CommandId == 0 && empId.Contains(x.Id) == true).ToList();
            ViewBag.WithoutCommand = withoutCommand;
            return View(commands);
        }

        [HttpPost]
        public IActionResult CreateCommand(Command command)
        {
            if (command.Name != "")
            {
                command.EmployerId = Convert.ToInt32(HttpContext.Session.GetString(SessionId));
                db.Commands.Add(command);
                db.SaveChanges();
            }
            return RedirectToAction("AllCommands");
        }
        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ResultOfSearch(string employee)
        {
            var emp = db.Employees.Where(x => x.EmployerId == Convert.ToInt32(HttpContext.Session.GetString(SessionId)))
                .Select(x => x.EmployeeId).ToList();
            var employees = db.UserProfiles.
                Where(u => (u.Login.StartsWith(employee) || u.FirstName.StartsWith(employee) || u.SecondName.StartsWith(employee))
                && emp.Contains(u.Id) == false).ToList();
            ViewBag.Search = employee;
            if (employees.Count != 0)
            {
                return View(employees);
            }
            else
            {
                return RedirectToAction("GetAll");
            }
        }
        [HttpGet]
        public IActionResult AddEmployee(int id)
        {
            var employee = new Employee() { EmployeeId = id, EmployerId = Convert.ToInt32(HttpContext.Session.GetString(SessionId)) };
            db.Employees.Add(employee);
            db.SaveChanges();
            return RedirectToAction("AllEmployees");
        }

        [HttpGet]
        public IActionResult DeleteEmployee(int userId)
        {
            var employee = db.Employees.Where(e => e.EmployeeId == userId).FirstOrDefault();
            var user = db.UserProfiles.Where(e => e.Id == userId).FirstOrDefault();
            var userCommand = db.UserCommands.Where(e => e.UserId == userId).FirstOrDefault();
            user.CommandId = 0;
            db.Employees.Remove(employee);
            db.UserCommands.Remove(userCommand);
            db.UserProfiles.Update(user);
            db.SaveChanges();
            return RedirectToAction("AllCommands");
        }
        [HttpPost]
        public IActionResult UpdatePosition(int id, string newPosition)
        {
            var user = db.UserProfiles.Where(u => u.Id == id).FirstOrDefault();
            user.Position = newPosition;
            db.UserProfiles.Update(user);
            db.SaveChanges();
            return RedirectToAction("AllPositions");
        }

        [HttpGet]
        public IActionResult DeleteCommand()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DeleteCommand(int commandId)
        {
            var command = db.Commands.Where(x => x.Id == commandId).FirstOrDefault();
            var userProfiles = db.UserProfiles.Where(x => x.CommandId == command.Id).ToList();
            var userCommands = db.UserCommands.Where(x => x.CommandId == command.Id).ToList();
            foreach (var p in userProfiles)
            {
                var newP = p;
                newP.CommandId = 0;
                db.UserProfiles.Update(newP);
            }
            db.SaveChanges();
            foreach (var c in userCommands)
            {
                db.UserCommands.Remove(c);
            }
            db.SaveChanges();
            db.Commands.Remove(command);
            db.SaveChanges();
            return RedirectToAction("AllCommands");
        }

        [HttpGet]
        public IActionResult UpdateCommand(int commandId)
        {
            var command = db.Commands.Where(x => x.Id == commandId).FirstOrDefault();
            return View(command);
        }

        [HttpGet]
        public IActionResult DeleteEmployees(int employeeId)
        {
            var id = Convert.ToInt32(HttpContext.Session.GetString(SessionId));
            var employee = db.Employees.Where(x => x.EmployeeId == employeeId && x.EmployerId == id).FirstOrDefault();
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("AllEmployees");
        }

        [HttpGet]
        public IActionResult AddToCommand(int commandId, int employeeId)
        {
            var user = db.UserProfiles.Where(x => x.Id == employeeId).FirstOrDefault();
            user.CommandId = commandId;
            db.UserProfiles.Update(user);
            db.UserCommands.Add(new UserCommand() { UserId = employeeId, CommandId = commandId });
            db.SaveChanges();

            return RedirectToAction("AllCommands");
        }

        [HttpPost]
        public IActionResult UpdateCommands(Command command)
        {
            var commandData = db.Commands.Where(x => x.Id == command.Id).FirstOrDefault();
            commandData.Name = command.Name;
            commandData.Description = command.Description;
            db.Commands.Update(commandData);
            db.SaveChanges();
            return RedirectToAction("AllCommands");
        }

        [HttpGet]
        public IActionResult AllEmployees()
        {
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                return RedirectToAction("Login", "Authentication");
            }
            var id = Convert.ToInt32(HttpContext.Session.GetString(SessionId));
            var empId = db.Employees.Where(x => x.EmployerId == id).Select(x => x.EmployeeId).ToList();
            List<UserProfile> employees = new List<UserProfile>();
            if (empId.Count() != 0)
            {
                employees = db.UserProfiles.Where(x => x.CommandId != 0 && empId.Contains(x.Id) == true).ToList();
            }
            var withoutCommand = db.UserProfiles.Where(x => x.CommandId == 0 && empId.Contains(x.Id) == true).ToList();

            ViewBag.Group = new List<int>();
            ViewBag.Free = new List<UserProfile>();
            var minAmount = db.UserProfiles.GroupBy(x => x.CommandId).Min(g => g.Count());
            var maxAmount = db.UserProfiles.GroupBy(x => x.CommandId).Min(g => g.Count());
            var commands = db.Commands.Where(x => x.EmployerId == id).Select(x => x.Id).ToList();
            List<string> warnings = new List<string>();
            List<string> groupsName = new List<string>();
            List<int> groups = new List<int>();
            List<int> notSignificant = new List<int>();
            if (employees.Count() != 0 && withoutCommand.Count() != 0 && minAmount > 1 && maxAmount > 1 && commands.Count() > 1)
            {
                var employee = employees[employees.Count() - 1];
                foreach (var c in commands)
                {
                    var ammount = db.UserProfiles.Where(x => x.CommandId == c).ToList().Count();
                    if (ammount <= 1)
                    {
                        commands.Remove(c);
                    }
                }
                BayesClassifier classifier = new BayesClassifier(employees, withoutCommand, commands);
                classifier.FindCommands();
                groups = classifier.FindPosteriorNumerator();
                notSignificant = classifier.NotSignificantColumns;
            }
            ViewBag.Group = groups;
            ViewBag.NotSignificant = notSignificant;
            ViewBag.Commands = db.Commands.Select(x => x.Name).ToList();
            ViewBag.Free = withoutCommand;

            FactorCorrelation factorCorrelation = new FactorCorrelation(employees);
            var order = factorCorrelation.OrderOfFactors();
            ViewBag.GroupTree = TreeOfDecision(withoutCommand, order, commands, employees);
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

        [HttpGet]
        public IActionResult AllPositions()
        {
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                return RedirectToAction("Login", "Authentication");
            }
            var id = Convert.ToInt32(HttpContext.Session.GetString(SessionId));
            var myEmployees = db.Employees.Where(x => x.EmployerId == id).Select(x => x.EmployeeId).ToList();
            ViewBag.WithPositions = db.UserProfiles.Where(x => myEmployees.Contains(x.Id) == true).ToList();
            var skills = db.EmployeeSkills.Where(x => myEmployees.Contains(x.EmployeeId) == true).ToList();
            var skillsOfGroup = new int[db.Groups.Count(), db.Skills.Count()];
            foreach (var s in db.Skills)
            {
                skillsOfGroup[s.GroupId - 1, s.Id - 1] = 1;
            }

            CMeans cMeans = new CMeans(skills, db.Skills.Count(), db.Groups.Count(), skillsOfGroup);
            List<int> newGroups = cMeans.FindClaster();

            List<UserSkillModel> userSkills = new List<UserSkillModel>();
            int i = 0;
            foreach (var e in myEmployees)
            {
                if (db.EmployeeSkills.Where(x => x.EmployeeId == e).ToList().Count() == 0)
                {
                    var u = db.UserProfiles.Where(x => x.Id == e).FirstOrDefault();
                    UserSkillModel usWith0 = new UserSkillModel(db.Skills.Count()) { Id = u.Id, FirstName = u.FirstName, SecondName = u.SecondName, Login = u.Login, Position = u.Position };
                    userSkills.Add(usWith0);
                    continue;
                }
                var user = db.UserProfiles.Where(x => x.Id == e).FirstOrDefault();
                UserSkillModel usm = new UserSkillModel(db.Skills.Count()) { Id = user.Id, FirstName = user.FirstName, SecondName = user.SecondName, Login = user.Login, Position = user.Position };
                usm.NewPosition = db.Groups.Where(x => x.Id == newGroups[i]).Select(x => x.GroupName).FirstOrDefault();
                foreach (var s in skills.Where(x => x.EmployeeId == user.Id).ToList())
                {
                    usm.SkillEvaluations[s.SkillId - 1] = s.Mark;
                }
                userSkills.Add(usm);
                i++;
            }
            ViewBag.UserSkills = userSkills;
            ViewBag.SkillNames = db.Skills.Select(x => x.SkillName).ToList();
            ViewBag.GroupNames = db.Groups.Select(x => x.GroupName).ToList();

            return View();
        }

        [HttpPost]
        public IActionResult UpdateCommand(int userId, int commandId)
        {
            var user = db.UserProfiles.Where(x => x.Id == userId).FirstOrDefault();
            user.CommandId = commandId;
            db.UserProfiles.Update(user);
            db.UserCommands.Add(new UserCommand() { UserId = userId, CommandId = commandId });
            db.SaveChanges();

            return RedirectToAction("AllEmployees");
        }
    }
}
