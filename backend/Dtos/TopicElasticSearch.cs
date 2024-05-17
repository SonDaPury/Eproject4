﻿using backend.Entities;

namespace backend.Dtos
{
    public class TopicElasticSearch
    {
        public int Id { get; set; }
        public required string TopicName { get; set; }
        public required SubTopcElasticSearch subTopics { get; set; }
    }
    public class SubTopcElasticSearch
    {
        public int Id { get; set; }
        public required string SubTopicName { get; set; }
        public required SourcesElasticSearch sources { get; set; }
    }
    public class SourcesElasticSearch
    {
        public int Id { get;set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Thumbnail { get; set; } = "";
        public string Slug { get; set; } = "";
        public int Status { get; set; }
        public string Benefit { get; set; } = "";
        public string Video_intro { get; set; } = "";
        public double Price { get; set; }
        public string Rating { get; set; } = "";
        public int UserId { get;set; }
    }
}
