﻿namespace NewsAggregator.WebAPI.Models.Requests
{
    public class GetArticleRequest
    {
        public string CategoryName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public float Rating { get; set; }
    }
}
