using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UNI_ASSETS.Models;
namespace UNI_ASSETS.Data
{
    /// <summary>
    /// Contains methods to add initial data into database
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        /// Adds default assets to the database and checks for any pending migrations
        /// </summary>
        /// <param name="app">Current running Application Instance</param>
        public static void AddDummy(IApplicationBuilder app)
        {
            IdentityContext context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<IdentityContext>();

            //if (context.Database.GetPendingMigrations().Any())
            //{
            //    context.Database.Migrate();
            //}
            context.Assets.AddRange(
     new Asset { AssetId = "Asset001", Name = "Desktop", Description = "Dell Latitude 5420", Default_Location = "IT Office",ImageUrl = "/Images/asset001.avif" },
     new Asset { AssetId = "Asset002", Name = "Projector", Description = "Epson Multimedia Projector", Default_Location = "Conference Room", ImageUrl = @"/Images/asset002.webp" },
     new Asset { AssetId = "Asset003", Name = "Printer", Description = "HP LaserJet Pro", Default_Location = "Admin Office", ImageUrl = @"/Images/asset003.jpg" },
     new Asset { AssetId = "Asset004", Name = "Desktop PC", Description = "HP EliteDesk Tower", Default_Location = "IT Office", ImageUrl = @"/Images/asset004.png" },
     new Asset { AssetId = "Asset005", Name = "Router", Description = "Cisco 2900 Series Router", Default_Location = "Server Room", ImageUrl = @"/Images/asset005.jpg" },
     new Asset { AssetId = "Asset006", Name = "Switch", Description = "Netgear 24-Port Switch", Default_Location = "Server Room", ImageUrl = @"/Images/asset006.jpeg" },
     new Asset { AssetId = "Asset007", Name = "Tablet", Description = "Samsung Galaxy Tab S7", Default_Location = "Training Room", ImageUrl = @"/Images/asset007.jpg" },
     new Asset { AssetId = "Asset008", Name = "Camera", Description = "Canon EOS DSLR", Default_Location = "Media Office", ImageUrl = @"/Images/asset008.jpg" },
     new Asset { AssetId = "Asset009", Name = "Scanner", Description = "Epson Flatbed Scanner", Default_Location = "Admin Office", ImageUrl = @"/Images/asset009.jpg" },
     new Asset { AssetId = "Asset010", Name = "Whiteboard", Description = "Magnetic Whiteboard 2m x 1m", Default_Location = "Conference Room", ImageUrl = @"/Images/asset010.webp" }
 );
            if(!context.Assets.Any())
            context.SaveChanges();

            
           
        }
        const string AdminUser = "App_Admin";
        const string AdminPass = "BlackSwatt99";
        const string AdminRole = "Admin";
        const string AdminEmail = "makubolebohang@gmail.com";

        /// <summary>
        /// Checks for any pending migrations and seeds the User Database with the admin user(programmer) and the 'Staff' role
        /// </summary>
        /// <param name="app">The Current running Application Instance</param>

        public async static void AddAdmin(IApplicationBuilder app)
        {
            IdentityContext context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<IdentityContext>();
            UserManager<AppUser> manager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            RoleManager<IdentityRole> RoleManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            //if (context.Database.GetPendingMigrations().Any())
            //{
            //    context.Database.Migrate();
            //}
            var Result = await manager.FindByEmailAsync(AdminEmail);
            if(Result == null)
            {
                AppUser user = new AppUser {UserName=AdminUser,Email=AdminEmail };
               var result =await manager.CreateAsync(user, AdminPass);
                if (result.Succeeded)
                {
                    if((await RoleManager.FindByNameAsync(AdminRole)) == null)
                    {
                      var task = await RoleManager.CreateAsync(new IdentityRole(AdminRole));
                        if (task.Succeeded)
                        {
                           await manager.AddToRoleAsync(user, AdminRole);
                        }
                    }
                }
                context.SaveChanges();
            }
            var role = await RoleManager.FindByNameAsync("Staff");
            if(role == null)
            {
               await RoleManager.CreateAsync(new IdentityRole("Staff"));
            }
        }
    }
}
