namespace HappyGift.Data.Models
{
    using HappyGift.Data.Common.Models;

    public class VoteCast : BaseModel<int>
    {
        public int VoteId { get; set; }

        public virtual Vote Vote { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int GiftId { get; set; }

        public virtual Gift Gift { get; set; }
    }
}
