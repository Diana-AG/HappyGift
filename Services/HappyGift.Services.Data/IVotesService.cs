namespace HappyGift.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using HappyGift.Data.Models;

    public interface IVotesService
    {
        Task StartVoteAsync(string startedByUserId, string userId);

        Task VoteForGiftAsync(int voteId, string userId, int giftId);

        Task EndVoteAsync(int voteId);

        Task<IEnumerable<Vote>> GetActiveVotesAsync(string userId);

        Task<Vote> GetVoteResultsAsync(int voteId);
    }
}
