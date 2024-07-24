namespace HappyGift.Web.ViewModels.Votes
{
    using System.Collections.Generic;

    public class VotesListViewModel
    {
        public string CurrentUserId { get; set; }

        public IEnumerable<VoteViewModel> Votes { get; set; }
    }
}
