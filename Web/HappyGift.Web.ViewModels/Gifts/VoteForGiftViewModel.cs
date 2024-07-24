namespace HappyGift.Web.ViewModels.Gifts
{
    using System.Collections.Generic;

    using HappyGift.Data.Models;
    using HappyGift.Services.Mapping;

    public class VoteForGiftViewModel : IMapFrom<Gift>
    {
        public int VoteId { get; set; }

        public IEnumerable<GiftViewModel> Gifts { get; set; }
    }
}
