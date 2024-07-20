namespace HappyGift.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using HappyGift.Data.Common.Repositories;
    using HappyGift.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class VotesService : IVotesService
    {
        private readonly IRepository<Vote> votesRepository;
        private readonly IRepository<VoteCast> voteCastsRepository;

        public VotesService(
            IRepository<Vote> votesRepository,
            IRepository<VoteCast> voteCastsRepository)
        {
            this.votesRepository = votesRepository;
            this.voteCastsRepository = voteCastsRepository;
        }

        public async Task EndVoteAsync(int voteId)
        {
            var vote = await this.votesRepository
                .AllAsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == voteId);

            if (vote == null || !vote.IsActive)
            {
                throw new InvalidOperationException("The vote is not active or does not exist.");
            }

            vote.EndVote();
            await this.votesRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Vote>> GetActiveVotesAsync(string userId)
        {
            return await this.votesRepository.AllAsNoTracking()
                .Where(v => v.IsActive && v.UserId != userId)
                .ToListAsync();
        }

        public async Task<Vote> GetVoteResultsAsync(int voteId)
        {
            var vote = await this.votesRepository
                .AllAsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == voteId);

            if (vote == null || vote.IsActive)
            {
                throw new InvalidOperationException("The vote is still active or does not exist.");
            }

            return vote;
        }

        public async Task StartVoteAsync(string startedByUserId, string userId)
        {
            var activeVote = await this.votesRepository.AllAsNoTracking()
            .AnyAsync(v => v.UserId == userId && v.IsActive);

            if (activeVote)
            {
                throw new InvalidOperationException("There is already an active vote for this user.");
            }

            var vote = new Vote
            {
                UserId = userId,
                StartedByUserId = startedByUserId,
                IsActive = true,
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
    }
}
