namespace HappyGift.Web.Controllers
{
    using System.Threading.Tasks;

    using HappyGift.Services.Data;
    using Microsoft.AspNetCore.Mvc;

    public class VoteController : BaseController
    {
        private readonly IVotesService votesService;

        public VoteController(IVotesService votesService)
        {
            this.votesService = votesService;
        }

        public async Task<IActionResult> StartVote(string userId)
        {
            var startedByUserId = this.User.Identity.Name;
            await this.votesService.StartVoteAsync(startedByUserId, userId);
            return this.RedirectToAction(nameof(HomeController.Index), nameof(HomeController));
        }

        public async Task<IActionResult> VoteForGift(int voteId, int giftId)
        {
            var userId = this.User.Identity.Name;
            await this.votesService.VoteForGiftAsync(voteId, userId, giftId);
            return this.RedirectToAction(nameof(HomeController.Index), nameof(HomeController));
        }

        public async Task<IActionResult> EndVote(int voteId)
        {
            await this.votesService.EndVoteAsync(voteId);
            return this.RedirectToAction(nameof(HomeController.Index), nameof(HomeController));
        }

        public async Task<IActionResult> ActiveVotes()
        {
            var userId = this.User.Identity.Name;
            var votes = await this.votesService.GetActiveVotesAsync(userId);
            return this.View(votes);
        }

        public async Task<IActionResult> VoteResults(int voteId)
        {
            var vote = await this.votesService.GetVoteResultsAsync(voteId);
            return this.View(vote);
        }
    }
}
