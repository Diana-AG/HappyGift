namespace HappyGift.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using HappyGift.Data.Common.Repositories;
    using HappyGift.Data.Models;
    using HappyGift.Services.Mapping;
    using HappyGift.Web.ViewModels.Gifts;
    using HappyGift.Web.ViewModels.Users;
    using HappyGift.Web.ViewModels.VoteCasts;
    using HappyGift.Web.ViewModels.Votes;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class VotesService : IVotesService
    {
        private readonly IRepository<Vote> votesRepository;
        private readonly IRepository<VoteCast> voteCastsRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public VotesService(
            IRepository<Vote> votesRepository,
            IRepository<VoteCast> voteCastsRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.votesRepository = votesRepository;
            this.voteCastsRepository = voteCastsRepository;
            this.userManager = userManager;
        }

        public async Task EndVoteAsync(int voteId)
        {
            var vote = await this.votesRepository
                .All()
                .FirstOrDefaultAsync(v => v.Id == voteId);

            if (vote == null || !vote.IsActive)
            {
                throw new InvalidOperationException("The vote is not active or does not exist.");
            }

            vote.EndVote();
            await this.votesRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<VoteViewModel>> GetAllAsync(string userId)
        {
            var votes = await this.votesRepository
                .AllAsNoTracking()
                .Where(v => v.ForUserId != userId)
                .To<VoteViewModel>()
                .OrderByDescending(v => v.IsActive)
                .ThenBy(v => v.ForBirthdayDate)
                .ToListAsync();

            foreach (var vote in votes)
            {
                vote.HasCurrentUserVoted = vote.VotesCast.Any(vc => vc.UserId == userId);
            }

            return votes;
        }

        public async Task<VoteDetailsViewModel> GetVoteResultsAsync(int voteId)
        {
            var vote = await this.votesRepository
                .AllAsNoTracking()
                .Where(v => v.Id == voteId)
                .To<VoteDetailsViewModel>()
                .FirstOrDefaultAsync();

            if (vote == null || vote.IsActive)
            {
                throw new InvalidOperationException("The vote is still active or does not exist.");
            }

            var giftVoteResults = vote.VotesCast
                .GroupBy(vc => vc.GiftName)
                .Select(g => new GiftVoteResulViewModel
                {
                    GiftName = g.Key,
                    VoteCastCount = g.Count(),
                })
                .OrderByDescending(x => x.VoteCastCount)
                .ThenBy(x => x.GiftName)
                .ToList();

            vote.GiftVoteResults = giftVoteResults;

            var users = await this.userManager.Users
                .Where(u => u.Id != vote.ForUserId)
                .Include(u => u.VoteCasts)
                .ThenInclude(vc => vc.Gift)
                .ToListAsync();

            var usersGifts = users.Select(u => new UserGiftViewModel
            {
                UserName = u.Name,
                GiftName = u.VoteCasts.FirstOrDefault(vc => vc.VoteId == voteId)?.Gift?.Name ?? "Not Voted",
                CreatedOn = u.VoteCasts.FirstOrDefault(vc => vc.VoteId == voteId)?.CreatedOn.ToString("d-MMMM-yyyy", CultureInfo.InvariantCulture) ?? string.Empty,
            });

            var giftOrder = giftVoteResults.Select((g, index) => new { g.GiftName, Index = index })
                                           .ToDictionary(x => x.GiftName, x => x.Index);

            vote.UsersGifts = usersGifts.OrderBy(ug => giftOrder.ContainsKey(ug.GiftName) ? giftOrder[ug.GiftName] : int.MaxValue)
                                        .ThenBy(ug => ug.UserName)
                                        .ToList();

            return vote;
        }

        public async Task StartVoteAsync(string startedByUserId, string userId)
        {
            var activeVote = await this.votesRepository
                .AllAsNoTracking()
                .AnyAsync(v => v.ForUserId == userId && v.IsActive);

            if (activeVote)
            {
                throw new InvalidOperationException("There is already an active vote for this user.");
            }

            var user = await this.userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            var currentDate = DateTime.UtcNow;
            var birthDate = user.BirthDate;

            int voteYear = (birthDate.Month > currentDate.Month ||
                    (birthDate.Month == currentDate.Month && birthDate.Day >= currentDate.Day))
                   ? currentDate.Year
                   : currentDate.Year + 1;

            var forBirthdayDate = new DateTime(voteYear, birthDate.Month, birthDate.Day);

            var existingVote = await this.votesRepository
                .AllAsNoTracking()
                .AnyAsync(v => v.ForUserId == userId && v.ForBirthdayDate == forBirthdayDate && v.IsActive);

            if (existingVote)
            {
                throw new InvalidOperationException("There is already an active vote for this birthday date.");
            }

            var vote = new Vote
            {
                ForUserId = userId,
                StartedByUserId = startedByUserId,
                IsActive = true,
                ForBirthdayDate = forBirthdayDate,
            };

            await this.votesRepository.AddAsync(vote);
            await this.votesRepository.SaveChangesAsync();
        }

        public async Task VoteForGiftAsync(int voteId, string userId, int giftId)
        {
            var vote = await this.votesRepository
                .AllAsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == voteId);

            if (vote == null || !vote.IsActive)
            {
                throw new InvalidOperationException("The vote is not active or does not exist.");
            }

            var existingVote = await this.voteCastsRepository.AllAsNoTracking()
                .AnyAsync(vc => vc.VoteId == voteId && vc.UserId == userId);

            if (existingVote)
            {
                throw new InvalidOperationException("You have already voted in this vote.");
            }

            var voteCast = new VoteCast
            {
                VoteId = voteId,
                UserId = userId,
                GiftId = giftId,
            };

            await this.voteCastsRepository.AddAsync(voteCast);
            await this.voteCastsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserViewModel>> GetAvailableUsersForVotingAsync(string currentUserId)
        {
            var currentDate = DateTime.UtcNow;

            var users = await this.userManager.Users
                .Where(u => u.Id != currentUserId)
                .ToListAsync();

            var usersWithActiveVotes = await this.votesRepository
                .AllAsNoTracking()
                .Where(v => v.IsActive)
                .Select(v => v.ForUserId)
                .ToListAsync();

            var filteredUsers = users
                .Where(u => !usersWithActiveVotes.Contains(u.Id) &&
                    !this.votesRepository.AllAsNoTracking()
                        .Any(v => v.ForUserId == u.Id && v.ForBirthdayDate == this.CalculateCommingBirthdayDate(u.BirthDate)))
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    Name = u.Name,
                    BirthDate = u.BirthDate,
                    CommingBirthdayDate = this.CalculateCommingBirthdayDate(u.BirthDate),
                });

            return filteredUsers;
        }

        private DateTime CalculateCommingBirthdayDate(DateTime birthDate)
        {
            var currentDate = DateTime.UtcNow;
            int voteYear = (birthDate.Month > currentDate.Month ||
                            (birthDate.Month == currentDate.Month && birthDate.Day >= currentDate.Day))
                           ? currentDate.Year
                           : currentDate.Year + 1;

            return new DateTime(voteYear, birthDate.Month, birthDate.Day);
        }
    }
}
