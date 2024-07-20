namespace HappyGift.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using HappyGift.Data.Common.Repositories;
    using HappyGift.Data.Models;
    using HappyGift.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class GiftsService : IGiftsService
    {
        private readonly IRepository<Gift> giftRepository;

        public GiftsService(IRepository<Gift> giftRepository)
        {
            this.giftRepository = giftRepository;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.giftRepository.All()
                .To<T>()
                .ToListAsync();
        }
    }
}
