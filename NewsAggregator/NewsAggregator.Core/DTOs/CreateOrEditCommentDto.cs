﻿namespace NewsAggregator.Core.DTOs
{
    public class CreateOrEditCommentDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid UserId { get; set; }
        public Guid ArticleId { get; set; }
    }
}
