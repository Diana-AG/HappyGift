namespace HappyGift.Web.ViewModels.Gifts
{
    using HappyGift.Data.Models;
    using HappyGift.Services.Mapping;

    public class GiftViewModel : IMapFrom<Gift>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
