namespace HappyGift.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using HappyGift.Data.Models;

    internal class GiftsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Gifts.Any())
            {
                return;
            }

            await dbContext.Gifts.AddRangeAsync(
                new Gift { Name = "E-Reader" },
                new Gift { Name = "Spa Gift Set" },
                new Gift { Name = "Chocolate Box" },
                new Gift { Name = "Designer Wallet" },
                new Gift { Name = "Bluetooth Speaker" });
        }
    }
}
