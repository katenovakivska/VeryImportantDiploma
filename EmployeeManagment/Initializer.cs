using EmployeeManagment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagment
{
    public class Initializer
    {
        public static void Initialize(EmployeeContext context)
        {
            //if (!context.Commands.Any())
            //{
            //    context.Commands.Add(
            //        new Command
            //        { 
            //            Name = "None"
            //        });
            //}
            if (!context.Groups.Any())
            {
                context.Groups.AddRange(
                    new Group
                    {
                        GroupName = "Front-end devs"
                    },
                    new Group
                    {
                        GroupName = "Back-end devs"
                    },
                    new Group
                    {
                        GroupName = "Game devs"
                    },
                    new Group
                    {
                        GroupName = "Android devs"
                    },
                    new Group
                    {
                        GroupName = "iOS devs"
                    }
                    ) ;
            }
            if (!context.Skills.Any())
            {
                context.Skills.AddRange(
                    new Skill
                    {
                        SkillName = "HTML/CSS",
                        GroupId = 1
                    },
                    new Skill
                    {
                        SkillName = "JavaScript",
                        GroupId = 1
                    },
                    new Skill
                    {
                        SkillName = "C#",
                        GroupId = 2
                    },
                    new Skill
                    {
                        SkillName = "Python",
                        GroupId = 2
                    },
                    new Skill
                    {
                        SkillName = "Java",
                        GroupId = 2
                    },
                    new Skill
                    {
                        SkillName = "Unity",
                        GroupId = 3
                    },
                    new Skill
                    {
                        SkillName = "Open GL",
                        GroupId = 3
                    },
                    new Skill
                    {
                        SkillName = "Unreal Engine",
                        GroupId = 3
                    },
                    new Skill
                    {
                        SkillName = "Android Studio",
                        GroupId = 4
                    },
                    new Skill
                    {
                        SkillName = "Android SDK",
                        GroupId = 4
                    },
                    new Skill
                    {
                        SkillName = "Swift",
                        GroupId = 5
                    },
                    new Skill
                    {
                        SkillName = "XCode",
                        GroupId = 5
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
