namespace BugTracker.Migrations
{
    using BugTracker.Models;
    using BugTracker.Models.Classes;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Blog.Models.ApplicationDbContext";
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            //role manager
            RoleManager<IdentityRole> roleManager =
               new RoleManager<IdentityRole>(
                   new RoleStore<IdentityRole>(context));

            //Admin role
            if (!context.Roles.Any(p => p.Name == "Admin"))
            {
                IdentityRole adminRole = new IdentityRole("Admin");
                roleManager.Create(adminRole);
            }

            //Project Manager role
            if (!context.Roles.Any(p => p.Name == "Project Manager"))
            {
                IdentityRole projectManagerRole = new IdentityRole("Project Manager");
                roleManager.Create(projectManagerRole);
            }

            //Developer role
            if (!context.Roles.Any(p => p.Name == "Developer"))
            {
                IdentityRole developerRole = new IdentityRole("Developer");
                roleManager.Create(developerRole);
            }

            //Submitter role
            if (!context.Roles.Any(p => p.Name == "Submitter"))
            {
                IdentityRole submitterRole = new IdentityRole("Submitter");
                roleManager.Create(submitterRole);
            }

            //user manager
            UserManager<ApplicationUser> userManager =
                new UserManager<ApplicationUser>(
                        new UserStore<ApplicationUser>(context));

            //Creating admin user
            ApplicationUser admin;

            if (!context.Users.Any(
                p => p.UserName == "admin@mybugtracker.com"))
            {
                admin = new ApplicationUser();
                admin.UserName = admin.Email = "admin@mybugtracker.com";
                admin.EmailConfirmed = true;
                admin.DisplayName = "Admin User";
                userManager.Create(admin, "Password-1");
            }
            else
            {
                admin = context
                    .Users
                    .First(p => p.UserName == "admin@mybugtracker.com");
            }

            //assigning admin role if not assigned
            if (!userManager.IsInRole(admin.Id, "Admin"))
            {
                userManager.AddToRole(admin.Id, "Admin");
            }

            //Demo Logins users

            //demo admin
            ApplicationUser demoAdmin;

            if (!context.Users.Any(p => p.Email == "DemoAdmin@mybugtracker.com"))
            {
                demoAdmin = new ApplicationUser
                {
                    Email = "DemoAdmin@mybugtracker.com",
                    UserName = "DemoAdmin@mybugtracker.com",
                    DisplayName = "DemoAdmin@mybugtracker.com",
                    EmailConfirmed = true
                };

                userManager.Create(demoAdmin, "Password-1");
            }
            else
            {
                demoAdmin = context.Users.First(p => p.Email == "DemoAdmin@mybugtracker.com");
            }

            //assigning admin role if not assigned
            if (!userManager.IsInRole(demoAdmin.Id, "Admin"))
            {
                userManager.AddToRole(demoAdmin.Id, "Admin");
            }

            //demo project manager
            ApplicationUser demoProjectManager;

            if (!context.Users.Any(p => p.Email == "DemoProjectManager@mybugtracker.com"))
            {
                demoProjectManager = new ApplicationUser
                {
                    Email = "DemoProjectManager@mybugtracker.com",
                    UserName = "DemoProjectManager@mybugtracker.com",
                    DisplayName = "DemoProjectManager@mybugtracker.com",
                    EmailConfirmed = true
                };

                userManager.Create(demoProjectManager, "Password-1");
            }
            else
            {
                demoProjectManager = context.Users.First(p => p.Email == "DemoProjectManager@mybugtracker.com");
            }

            //assigning project manager role if not assigned
            if (!userManager.IsInRole(demoProjectManager.Id, "Project Manager"))
            {
                userManager.AddToRole(demoProjectManager.Id, "Project Manager");
            }

            //demo submitter
            ApplicationUser demoSubmitter;

            if (!context.Users.Any(p => p.Email == "DemoSubmitter@mybugtracker.com"))
            {
                demoSubmitter = new ApplicationUser
                {
                    Email = "DemoSubmitter@mybugtracker.com",
                    UserName = "DemoSubmitter@mybugtracker.com",
                    DisplayName = "DemoSubmitter@mybugtracker.com",
                    EmailConfirmed = true
                };

                userManager.Create(demoSubmitter, "Password-1");
            }
            else
            {
                demoSubmitter = context.Users.First(p => p.Email == "DemoSubmitter@mybugtracker.com");
            }

            //assigning submitter role if not assigned
            if (!userManager.IsInRole(demoSubmitter.Id, "Submitter"))
            {
                userManager.AddToRole(demoSubmitter.Id, "Submitter");
            }

            //demo developer
            ApplicationUser demoDeveloper;

            if (!context.Users.Any(p => p.Email == "DemoDeveloper@mybugtracker.com"))
            {
                demoDeveloper = new ApplicationUser
                {
                    Email = "DemoDeveloper@mybugtracker.com",
                    UserName = "DemoDeveloper@mybugtracker.com",
                    DisplayName = "DemoDeveloper@mybugtracker.com",
                    EmailConfirmed = true
                };

                userManager.Create(demoDeveloper, "Password-1");
            }
            else
            {
                demoDeveloper = context.Users.First(p => p.Email == "DemoDeveloper@mybugtracker.com");
            }

            //assigning submitter role if not assigned
            if (!userManager.IsInRole(demoDeveloper.Id, "Developer"))
            {
                userManager.AddToRole(demoDeveloper.Id, "Developer");
            }

            //Seeding Ticket Types, Priorities and Statuses.

            //Tickets
            context.TicketTypes.AddOrUpdate(p => p.Name, new TicketType() { Name = "Bug" }, new TicketType() { Name = "Feature" }, new TicketType() { Name = "Database" }, new TicketType() { Name = "Support" });

            //Priorities
            context.TicketPriorities.AddOrUpdate(p => p.Name, new TicketPriority() { Name = "Low" }, new TicketPriority() { Name = "Medium" }, new TicketPriority() { Name = "High" });

            //Statuses
            context.TicketStatuses.AddOrUpdate(p => p.Name, new TicketStatus() { Name = "Open" }, new TicketStatus() { Name = "Resolved" }, new TicketStatus() { Name = "Rejected" });

            context.SaveChanges();
        }
    }
}
