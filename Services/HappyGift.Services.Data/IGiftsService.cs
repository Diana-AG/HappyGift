namespace HappyGift.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGiftsService
    {
        Task<IEnumerable<T>> GetAllAsync<T>();
    }
}
