using System;
using System.Collections.Generic;
using EmployeeManagment.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EmployeeManagment.Methods;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Newtonsoft.Json;
using Extreme.DataAnalysis;
using Extreme.Mathematics;
using Extreme.Statistics;
using Extreme.Statistics.TimeSeriesAnalysis;

namespace EmployeeManagment.Controllers
{
    public class TaskController : Controller
    {
        EmployeeContext db;
        const string SessionName = "_Name";
        const string SessionId = "_Id";
        public TaskController(EmployeeContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult CommandBoard(int commandId)
        {
            var tasks = db.Task.Where(i => i.CommandId == commandId && i.Status != "closed").OrderBy(x => x.Start).ToList();
            foreach (var t in tasks)
            {
                t.Employee = db.UserProfiles.Where(x => x.Id == t.EmployeeId).FirstOrDefault();
                if (t.Employee.Picture != null)
                {
                    string imageBase64Data = Convert.ToBase64String(t.Employee.Picture);
                    string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                    t.Employee.PictureConverted = imageDataURL;
                }
            }
            ViewBag.CommandId = commandId;
            return View(tasks);
        }

        [HttpGet]
        public IActionResult UpdateTask(int taskId)
        {
            var task = db.Task.Where(t => t.Id == taskId).FirstOrDefault();
            task.Employee = db.UserProfiles.Where(e => e.Id == task.EmployeeId).FirstOrDefault();
            var id = Convert.ToInt32(HttpContext.Session.GetString(SessionId));
            var employees = db.UserProfiles.Where(e => e.CommandId == task.CommandId || e.Id == id).ToList();
            ViewBag.Employees = new List<UserProfile>();
            ViewBag.Employees = employees;
            return View(task);
        }
        [HttpPost]
        public IActionResult UpdateTask(Models.Task task)
        {
            var t = db.Task.Where(t => t.Id == task.Id).FirstOrDefault();
            t.EmployeeId = task.EmployeeId;
            t.Status = task.Status;
            t.Description = task.Description;
            t.End = task.End;
            t.Complexity = task.Complexity;
            db.Task.Update(t);
            db.SaveChanges();
            return RedirectToAction("CommandBoard", new { commandId = t.CommandId });
        }

        [HttpGet]
        public IActionResult AddTask(int commandId)
        {
            ViewBag.CommandId = commandId;
            var id = Convert.ToInt32(HttpContext.Session.GetString(SessionId));
            var employees = db.UserProfiles.Where(e => e.CommandId == commandId || e.Id == id).ToList();
            ViewBag.Employees = new List<UserProfile>();
            ViewBag.Employees = employees;
            ViewBag.Employee = db.UserProfiles.Where(e => e.Id == id).FirstOrDefault();
            return View();
        }

        [HttpPost]
        public IActionResult AddTask(Models.Task task)
        {
            task.Start = DateTime.Now;
            db.Task.Add(task);
            db.SaveChanges();
            return RedirectToAction("CommandBoard", new { commandId = task.CommandId });
        }

        [HttpGet]
        public IActionResult CloseTask(int taskId)
        {
            var task = db.Task.Where(t => t.Id == taskId).FirstOrDefault();
            task.Status = "closed";
            db.Task.Update(task);
            db.SaveChanges();
            return RedirectToAction("CommandBoard", new { commandId = task.CommandId });
        }

        [HttpGet]
        public IActionResult Forecast(int commandId, int amountOfWeeks = 3)
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
                        avgComplexity = db.Task.Where(t => t.CommandId == commandId && t.End >= startDate && t.End <= endDate).Sum(x => x.Complexity) / amount;
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
            //arima
            ArimaModel model = new ArimaModel(historyAmount, 2, 1);
            model.Compute();
            var forecast = model.Forecast(amountOfWeeks);
            ArimaModel model1 = new ArimaModel(historyComplexity, 2, 1);
            model1.Compute();
            var forecast1 = model1.Forecast(amountOfWeeks);
            List<DataPoint> forecastPoints = new List<DataPoint>();
            var lastDate = new DateTime(points.Last().Year, points.Last().Month, points.Last().Day);
            lastDate = lastDate.AddDays(1);
            //exponential smoothing
            List<DataPoint> forecastPoints1 = new List<DataPoint>();
            var model2 = new ExponentialSmoothingModel(historyAmount, ExponentialSmoothingMethod.Double);
            model2.TrendEstimator = ExponentialSmoothingTrendEstimator.Complete;
            model2.Compute();
            var forecast2 = model2.Forecast(amountOfWeeks);
            var model3 = new ExponentialSmoothingModel(historyComplexity, ExponentialSmoothingMethod.Double);
            model3.TrendEstimator = ExponentialSmoothingTrendEstimator.Complete;
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
            JsonSerializerSettings jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
            ViewBag.DataPoints = JsonConvert.SerializeObject(points, jsonSetting);
            ViewBag.ForecastPoints = JsonConvert.SerializeObject(forecastPoints, jsonSetting);
            ViewBag.ForecastPoints1 = JsonConvert.SerializeObject(forecastPoints1, jsonSetting);
            ViewBag.CommandId = commandId;
            return View();
        }

    }


}
