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
                admin.UserName = admin.Email = admin.DisplayName = "admin@mybugtracker.com";
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
