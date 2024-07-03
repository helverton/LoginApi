using Microsoft.AspNetCore.Identity;

namespace LoginApi.Data
{
    public class DatabaseInitializer
    {
        public static async Task SeedDateAsync(UserManager<IdentityUser>? userManager, RoleManager<IdentityRole>? roleManager)
        {
            if (userManager == null || roleManager == null)
            {
                Console.WriteLine("UserManager or roleManager is null => exit");
                return;
            }


            var exists = false;

            // check if we have the system role or not
            exists = await roleManager.RoleExistsAsync("system");
            if (!exists)
            {
                Console.WriteLine("System role is not defined and will be created");
                await roleManager.CreateAsync(new IdentityRole("System"));
            }

            // check if we have the admin role or not
            exists = await roleManager.RoleExistsAsync("admin");
            if (!exists)
            {
                Console.WriteLine("Admin role is not defined and will be created");
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            // check if we have the user role or not
            exists = await roleManager.RoleExistsAsync("user");
            if (!exists)
            {
                Console.WriteLine("User role is not defined and will be created");
                await roleManager.CreateAsync(new IdentityRole("user"));
            }

            // check if we have the seller role or not
            exists = await roleManager.RoleExistsAsync("seller");
            if (!exists)
            {
                Console.WriteLine("Seller role is not defined and will be created");
                await roleManager.CreateAsync(new IdentityRole("seller"));
            }


            // create the super user
            var user = new IdentityUser()
            {
                UserName = "system@email.com.br",
                Email = "system@email.com.br"
            };

            string defaultPassword = "System@123";

            var result = await userManager.CreateAsync(user, defaultPassword);
            if (result.Succeeded)
            {
                // set the user role
                await userManager.AddToRoleAsync(user, "system");
                Console.WriteLine("System user created successfully! Please update the initial password!");
                Console.WriteLine("Email: " + user.Email + " - Initial password: " + defaultPassword);
            }
            else
            {
                Console.WriteLine("Unable to create System user: " + result.Errors.First().Description);
            }


            // create the bbz admin user
            user = new IdentityUser()
            {
                UserName = "admin@email.com.br",
                Email = "admin@email.com.br"
            };

            defaultPassword = "Admin@123";

            result = await userManager.CreateAsync(user, defaultPassword);
            if (result.Succeeded)
            {
                // set the user role
                await userManager.AddToRoleAsync(user, "admin");
                Console.WriteLine("Admin user created successfully! Please update the initial password!");
                Console.WriteLine("Email: " + user.Email + " - Initial password: " + defaultPassword);
            }
            else
            {
                Console.WriteLine("Unable to create Admin user: " + result.Errors.First().Description);
            }
        }
    }
}
