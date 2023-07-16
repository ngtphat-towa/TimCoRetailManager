namespace TRMDataManager.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TRMDataManager.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            // Set this into true
            // Open the Package Manager Console
            // Type: Enable-Migrations
            // Then type: Update-Database -Verbose
            // Open the API and create a new account manually
            // Change the  User ID associated with this account in TRMData
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TRMDataManager.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
