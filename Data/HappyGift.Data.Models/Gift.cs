namespace HappyGift.Data.Models
{
    using System.Collections.Generic;

    using HappyGift.Data.Common.Models;

    public class Gift : BaseModel<int>
    {
        public Gift()
        {
            this.VoteCasts = new HashSet<VoteCast>();
        }

        public string Name { get; set; }

        public virtual ICollection<VoteCast> VoteCasts { get; set; }
    }
}
