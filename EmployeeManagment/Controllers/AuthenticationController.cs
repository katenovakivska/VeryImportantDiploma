using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace EmployeeManagment.Controllers
{
    public class AuthenticationController : Controller
    {
        EmployeeContext db;
        private UserManager<ApplicationUser> _userManager;
        private readonly ApplicationSettings _applicationSettings;
        const string SessionName = "_Name";
        const string SessionId = "_Id";
        public AuthenticationController(UserManager<ApplicationUser> userManager, EmployeeContext employee, IOptions<ApplicationSettings> options)
        {
            _userManager = userManager;
            _applicationSettings = options.Value;
            db = employee;
        }

        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<Object> RegisterUser(ApplicationUserModel model)
        {
            var applicationUser = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email
            };
            var userProfile = new UserProfile()
            {
                Email = model.Email,
                Login = model.UserName,
                Password = model.Password
            };
            try
            {
                var result = await _userManager.CreateAsync(applicationUser, model.Password);
                db.UserProfiles.Add(userProfile);
                await db.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {

            var user = db.UserProfiles.Where(x => x.Login == model.UserName).FirstOrDefault();
            bool isLogin = (model.Password == user.Password) ? true : false;
            if (user != null && isLogin)
            {


                /*var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID",user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1234567890123456")), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);*/
                HttpContext.Session.SetString(SessionName, user.Login);
                int id = db.UserProfiles.Where(x => x.Login == user.Login).FirstOrDefault().Id;
                HttpContext.Session.SetString(SessionId, id.ToString());
                return RedirectToAction("UserProfile");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        [HttpGet]
        public IActionResult UserProfile()
        {
            var userName = HttpContext.Session.GetString(SessionName);
            if (userName == null)
            {
                return RedirectToAction("Login");
            }
            
            var user = db.UserProfiles.Where(x => x.Login == userName).FirstOrDefault();
            if (user.Picture != null)
            {
                string imageBase64Data = Convert.ToBase64String(user.Picture);
                string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                user.PictureConverted = imageDataURL;
            }
            var id = Convert.ToInt32(HttpContext.Session.GetString(SessionId));
            var skills = db.EmployeeSkills.Where(x => x.EmployeeId == id).ToList();
            var skillsList = db.Skills.ToList();
            ViewBag.EmployeeSkills = skills;
            ViewBag.Skills = skillsList;
            return View(user);
        }

        [HttpGet]
        public IActionResult UpdateProfile(int id)
        {
            var user = db.UserProfiles.Where(u => u.Id == id).FirstOrDefault();
            return View(user);
        }

        [HttpPost]
        public IActionResult UpdateProfile(int id, UserProfile model)
        {
            var user = db.UserProfiles.Where(u => u.Id == id).FirstOrDefault();
            user.Level = model.Level;
            user.AbilityToWorkInHome = model.AbilityToWorkInHome;
            user.AbilityToWorkInOffice = model.AbilityToWorkInOffice;
            user.Experience = model.Experience;
            user.FirstName = model.FirstName;
            user.SecondName = model.SecondName;
            user.Email = model.Email;
            user.Login = model.Login;
            user.Password = model.Password;
            user.Picture = user.Picture;
            user.AverageComplexityOfTasks = model.AverageComplexityOfTasks;
            user.CommandId = model.CommandId;
            db.UserProfiles.Update(user);
            db.SaveChanges();
            return RedirectToAction("UserProfile");
        }
        [HttpPost]
        public IActionResult ChangePhoto(int id, IFormFile newPicture)
        {
            var avatar = newPicture;
            // Person person = new Person { Name = pvm.Name };
            var user = db.UserProfiles.Where(u => u.Id == id).FirstOrDefault();
            if (avatar == null)
            {
                return null;
            }

            byte[] imageData = null;
            // считываем переданный файл в массив байтов
            using (var binaryReader = new BinaryReader(avatar.OpenReadStream()))
            {
                imageData = binaryReader.ReadBytes((int)avatar.Length);
            }
            // установка массива байтов

            user.Picture = imageData;
            db.UserProfiles.Update(user);
            db.SaveChanges();

            return RedirectToAction("UserProfile", new { userName = user.Login});
        }

        [HttpGet]
        public IActionResult UpdateSkills(int id)
        {
            var skills = db.Skills.ToList();
            ViewBag.Skills = skills;
            var userSkills = db.EmployeeSkills.Where(x => x.EmployeeId == id).ToList();
            List<int> marks = new List<int>();
            for(int i = 0; i < skills.Count(); i++)
            {
                var mark = userSkills.Where(x => x.SkillId == skills[i].Id).FirstOrDefault();
                if (mark != null)
                {
                    marks.Add(mark.Mark);
                }
                else
                {
                    marks.Add(0);
                }
            }
            ViewBag.Marks = marks;
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        public IActionResult UpdateSkills(int id, int AndroidSDK, int AndroidStudio, int UnrealEngine, int OpenGL, int Unity, int Python, int Swift, 
            int C, int JavaScript, int HTMLCSS, int Java, int XCode)
        {
            UpdateSkill(new EmployeeSkill() { EmployeeId = id, SkillId = 1, Mark = AndroidSDK });
            UpdateSkill(new EmployeeSkill() { EmployeeId = id, SkillId = 2, Mark = AndroidStudio });
            UpdateSkill(new EmployeeSkill() { EmployeeId = id, SkillId = 3, Mark = UnrealEngine });
            UpdateSkill(new EmployeeSkill() { EmployeeId = id, SkillId = 4, Mark = OpenGL });
            UpdateSkill(new EmployeeSkill() { EmployeeId = id, SkillId = 5, Mark = Unity });
            UpdateSkill(new EmployeeSkill() { EmployeeId = id, SkillId = 6, Mark = Python });
            UpdateSkill(new EmployeeSkill() { EmployeeId = id, SkillId = 7, Mark = Swift });
            UpdateSkill(new EmployeeSkill() { EmployeeId = id, SkillId = 8, Mark = C });
            UpdateSkill(new EmployeeSkill() { EmployeeId = id, SkillId = 9, Mark = JavaScript });
            UpdateSkill(new EmployeeSkill() { EmployeeId = id, SkillId = 10, Mark = HTMLCSS });
            UpdateSkill(new EmployeeSkill() { EmployeeId = id, SkillId = 11, Mark = Java });
            UpdateSkill(new EmployeeSkill() { EmployeeId = id, SkillId = 12, Mark = XCode });
            return RedirectToAction("UserProfile");
        }

        public void UpdateSkill(EmployeeSkill es)
        {
            if (es.Mark != 0)
            {
                var skill = db.EmployeeSkills.Where(x => x.EmployeeId == es.EmployeeId && x.SkillId == es.SkillId).FirstOrDefault();
                if (skill == null)
                {
                    db.EmployeeSkills.Add(es);
                }
                else
                {
                    skill.Mark = es.Mark;
                    db.EmployeeSkills.Update(skill);
                }
                db.SaveChanges();
            }
        }
    }
}
