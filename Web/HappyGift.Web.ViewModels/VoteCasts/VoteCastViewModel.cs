namespace HappyGift.Web.ViewModels.VoteCasts
{
    using HappyGift.Data.Models;
    using HappyGift.Services.Mapping;

    public class VoteCastViewModel : IMapFrom<VoteCast>
    {
        public string UserId { get; set; }

        public string GiftName { get; set; }
    }
}
