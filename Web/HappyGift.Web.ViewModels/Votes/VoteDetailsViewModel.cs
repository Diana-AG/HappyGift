namespace HappyGift.Web.ViewModels.Votes
{
    using System;
    using System.Collections.Generic;

    using HappyGift.Data.Models;
    using HappyGift.Services.Mapping;
    using HappyGift.Web.ViewModels.Gifts;
    using HappyGift.Web.ViewModels.Users;
    using HappyGift.Web.ViewModels.VoteCasts;

    public class VoteDetailsViewModel : IMapFrom<Vote>
    {
        public int Id { get; set; }

        public string ForUserId { get; set; }

        public string ForUserName { get; set; }

        public DateTime ForBirthdayDate { get; set; }

        public string StartedByUserName { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? EndedAt { get; set; }

        public bool IsActive { get; set; }

        public int VotesCastCount { get; set; }

        public IEnumerable<VoteCastViewModel> VotesCast { get; set; }

        public IEnumerable<UserGiftViewModel> UsersGifts { get; set; }

        public IEnumerable<GiftVoteResulViewModel> GiftVoteResults { get; set; }
    }
}
