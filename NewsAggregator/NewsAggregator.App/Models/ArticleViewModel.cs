﻿namespace NewsAggregator.App.Models
{
    public class ArticleViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public DateTime CreationDate { get; set; }
        public float Rating { get; set; }
    }
}
