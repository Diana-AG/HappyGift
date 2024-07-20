namespace HappyGift.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using HappyGift.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    internal class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            foreach (var user in UserSeederConstants.Users)
            {
                await CreateUser(userManager, user.UserName, user.Name, user.BirthDate);
            }
        }

        private static async Task CreateUser(
            UserManager<ApplicationUser> userManager,
            string userName,
            string name,
            DateTime birthDate)
        {
            if (userManager.Users.Any(x => x.UserName == userName))
            {
                return;
            }

            var user = new ApplicationUser
            {
                UserName = userName,
                Name = name,
                BirthDate = birthDate,
            };

            await userManager.CreateAsync(user, UserSeederConstants.DefaultPassword);
        }
    }
}
