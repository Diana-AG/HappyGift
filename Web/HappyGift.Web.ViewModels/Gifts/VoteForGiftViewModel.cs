namespace HappyGift.Web.ViewModels.Gifts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using HappyGift.Data.Models;
    using HappyGift.Services.Mapping;

    public class VoteForGiftViewModel : IMapFrom<Gift>
    {
        public int VoteId { get; set; }

        [Required(ErrorMessage = "Please select a gift.")]
        public int? GiftId { get; set; }

        public string UserName { get; set; }

        public IEnumerable<GiftViewModel> Gifts { get; set; }
    }
}
