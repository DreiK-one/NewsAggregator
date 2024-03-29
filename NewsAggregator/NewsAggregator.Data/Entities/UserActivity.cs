﻿namespace NewsAggregator.Data.Entities
{
    public class UserActivity : BaseEntity
    {
        public int NumberOfViews { get; set; }
        public DateTime ViewingDate { get; set; }


        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid ArticleId { get; set; }
        public virtual Article Article { get; set; }
    }
}
