using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagment.Controllers
{
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            //_signInManager = signInManager;
        }
        
        [Authorize]
        [HttpGet]
        public IActionResult UserProfile()
        {
            var user = _userManager.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            var appUser = new ApplicationUserModel();
            appUser.Email = user.Email;
            appUser.UserName = user.UserName;
            return Ok(appUser);
        }
    }

    
}
