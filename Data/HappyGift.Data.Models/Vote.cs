namespace HappyGift.Data.Models
{
    using System;
    using System.Collections.Generic;

    using HappyGift.Data.Common.Models;

    public class Vote : BaseModel<int>
    {
        private DateTime? endedAt;

        public Vote()
        {
            this.VotesCast = new HashSet<VoteCast>();
        }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string StartedByUserId { get; set; }

        public virtual ApplicationUser StartedByUser { get; set; }

        public bool IsActive { get; set; }

        public DateTime? EndedAt
        {
            get => this.endedAt;
            set
            {
                this.endedAt = value;
                this.IsActive = !value.HasValue;
            }
        }

        public virtual ICollection<VoteCast> VotesCast { get; set; }

        public void EndVote()
        {
            this.EndedAt = DateTime.UtcNow;
        }
    }
}
