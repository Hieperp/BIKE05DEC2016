namespace MVCClient.Migrations
{
    using MVCClient.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MVCClient.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "MVCClient.Models.ApplicationDbContext";
        }

        protected override void Seed(MVCClient.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //this.AddUserAndRoles1();
        }


        bool AddUserAndRoles1()
        {

            bool success = false;



            var idManager = new IdentityManager();


            success = idManager.CreateRole("Admin");

            if (!success == true) return success;



            success = idManager.CreateRole("CanEdit");

            if (!success == true) return success;



            success = idManager.CreateRole("User");

            if (!success) return success;





            


            success = idManager.AddUserToRole("11bf0c11-cc6d-4d78-8d24-c226c11cf89d", "Admin");

            if (!success) return success;



            success = idManager.AddUserToRole("11bf0c11-cc6d-4d78-8d24-c226c11cf89d", "CanEdit");

            if (!success) return success;



            success = idManager.AddUserToRole("11bf0c11-cc6d-4d78-8d24-c226c11cf89d", "User");

            if (!success) return success;



            return success;

        }


        bool AddUserAndRoles()
        {

            bool success = false;



            var idManager = new IdentityManager();


            success = idManager.CreateRole("Admin");

            if (!success == true) return success;



            success = idManager.CreateRole("CanEdit");

            if (!success == true) return success;



            success = idManager.CreateRole("User");

            if (!success) return success;





            var newUser = new ApplicationUser()

            {
                Email = "tanthanhhotel@gmail.com",

                FirstName = "Tan Thanh",

                LastName = "Admin"

            };



            // Be careful here - you  will need to use a password which will 

            // be valid under the password rules for the application, 

            // or the process will abort:

            success = idManager.CreateUser(newUser, "TanThanh@014");

            if (!success) return success;



            success = idManager.AddUserToRole(newUser.Id, "Admin");

            if (!success) return success;



            success = idManager.AddUserToRole(newUser.Id, "CanEdit");

            if (!success) return success;



            success = idManager.AddUserToRole(newUser.Id, "User");

            if (!success) return success;



            return success;

        }


    }
}
