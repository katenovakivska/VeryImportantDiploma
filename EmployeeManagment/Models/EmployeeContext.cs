using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagment.Models
{
    public class EmployeeContext: IdentityDbContext
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<UserCommand> UserCommands { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeSkill> EmployeeSkills { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Task> Task { get; set; }
        public EmployeeContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserCommand>()
                .HasKey(uc => new { uc.UserId, uc.CommandId });
            builder.Entity<UserCommand>()
                .HasOne(uc => uc.Command)
                .WithMany(c => c.UserCommands)
                .HasForeignKey(uc => uc.CommandId);
            builder.Entity<UserCommand>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserCommands)
                .HasForeignKey(uc => uc.UserId);

            builder.Entity<UserProfile>()
                .HasMany(u => u.Employees)
                .WithOne(e => e.Employer);


            builder.Entity<Command>()
                .HasMany(e => e.UserCommands)
                .WithOne(c => c.Command);

            builder.Entity<UserProfile>()
                .HasMany(c => c.Skills)
                .WithOne(e => e.Employee);

            builder.Entity<Group>()
                .HasMany(c => c.Skills)
                .WithOne(e => e.Group);

            builder.Entity<UserProfile>()
                .HasMany(c => c.Tasks)
                .WithOne(e => e.Employee);
        }
    }
}
