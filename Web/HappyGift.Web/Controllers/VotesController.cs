namespace HappyGift.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using HappyGift.Data.Models;
    using HappyGift.Services.Data;
    using HappyGift.Web.ViewModels.Gifts;
    using HappyGift.Web.ViewModels.Users;
    using HappyGift.Web.ViewModels.Votes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class VotesController : BaseController
    {
        private readonly IVotesService votesService;
        private readonly IGiftsService giftsService;
        private readonly UserManager<ApplicationUser> userManager;

        public VotesController(IVotesService votesService, IGiftsService giftsService, UserManager<ApplicationUser> userManager)
        {
            this.votesService = votesService;
            this.userManager = userManager;
            this.giftsService = giftsService;
        }

        [HttpGet]
        public async Task<IActionResult> StartVote()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user.Id == null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            var availableUsers = await this.votesService.GetAvailableUsersForVotingAsync(user.Id);
            var viewModel = new UsersListViewModel { Users = availableUsers };
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> StartVote(UsersListViewModel model)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            if (!this.ModelState.IsValid)
            {
                var availableUsers = await this.votesService.GetAvailableUsersForVotingAsync(currentUser.Id);
                var viewModel = new UsersListViewModel { Users = availableUsers };
                return this.View(viewModel);
            }

            if (currentUser.Id == model.SelectedUserId)
            {
                return this.BadRequest("You cannot start a vote for yourself.");
            }

            try
            {
                await this.votesService.StartVoteAsync(currentUser.Id, model.SelectedUserId);
                this.TempData["SuccessMessage"] = "The vote has been successfully started!";
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> VoteForGift(int voteId)
        {
            var gifts = await this.giftsService.GetAllAsync<GiftViewModel>();
            var vote = await this.votesService.GetVoteByIdAsync<VoteDetailsViewModel>(voteId);

            var model = new VoteForGiftViewModel
            {
                VoteId = voteId,
                Gifts = gifts,
                UserName = vote.ForUserName,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> VoteForGift(int voteId, int giftId)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (giftId <= 0)
            {
                this.ModelState.AddModelError(string.Empty, "Please select a gift.");
                var gifts = await this.giftsService.GetAllAsync<GiftViewModel>();
                var model = new VoteForGiftViewModel { VoteId = voteId, Gifts = gifts };
                return this.View(model);
            }

            try
            {
                await this.votesService.VoteForGiftAsync(voteId, user.Id, giftId);
                this.TempData["SuccessMessage"] = "Your vote has been recorded.";
                return this.RedirectToAction(nameof(this.Index));
            }
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
            }

            var allGifts = await this.giftsService.GetAllAsync<GiftViewModel>();
            var viewModel = new VoteForGiftViewModel { VoteId = voteId, Gifts = allGifts };
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EndVote(int voteId)
        {
            await this.votesService.EndVoteAsync(voteId);
            this.TempData["SuccessMessage"] = "The vote has been successfully ended.";
            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var votes = await this.votesService.GetAllAsync(user.Id);
            var model = new VotesListViewModel { Votes = votes, CurrentUserId = user.Id };
            return this.View(model);
        }

        public async Task<IActionResult> VoteResults(int voteId)
        {
            try
            {
                var vote = await this.votesService.GetVoteResultsAsync(voteId);
                return this.View(vote);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}
