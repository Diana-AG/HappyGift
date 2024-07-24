namespace HappyGift.Web.ViewModels.Votes
{
    using System;
    using System.Collections.Generic;

    using HappyGift.Data.Models;
    using HappyGift.Services.Mapping;
    using HappyGift.Web.ViewModels.VoteCasts;

    public class VoteViewModel : IMapFrom<Vote>
    {
        public int Id { get; set; }

        public string ForUserName { get; set; }

        public DateTime ForBirthdayDate { get; set; }

        public string StartedByUserId { get; set; }

        public bool IsActive { get; set; }

        public bool HasCurrentUserVoted { get; set; }

        public int VotesCastCount { get; set; }

        public IEnumerable<VoteCastViewModel> VotesCast { get; set; }
    }
}
