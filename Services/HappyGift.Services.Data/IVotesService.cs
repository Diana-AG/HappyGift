namespace HappyGift.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using HappyGift.Web.ViewModels.Users;
    using HappyGift.Web.ViewModels.Votes;

    public interface IVotesService
    {
        Task StartVoteAsync(string startedByUserId, string userId);

        Task VoteForGiftAsync(int voteId, string userId, int giftId);

        Task EndVoteAsync(int voteId);

        Task<T> GetVoteByIdAsync<T>(int voteId);

        Task<VoteDetailsViewModel> GetVoteResultsAsync(int voteId);

        Task<IEnumerable<VoteViewModel>> GetAllAsync(string userId);

        Task<IEnumerable<UserViewModel>> GetAvailableUsersForVotingAsync(string currentUserId);
    }
}
