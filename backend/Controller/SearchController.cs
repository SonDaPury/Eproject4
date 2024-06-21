using Azure;
using backend.Data;
using backend.Dtos;
using backend.Entities;
using backend.Service.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System.Net.WebSockets;

namespace backend.Controller
{
    public class SearchQuery
    {
        public string TopicName { get; set; }
        public string SubTopicName { get; set; }
        public int? RatingLte { get; set; }
        public int? RatingGte { get; set; }
        public int? PriceGte { get; set; }
        public int? PricedLte { get; set; }
    }
    [Route("api/[controller]")]
    public class SearchController
    {
        private readonly IElasticSearchRepository _elasticSearchRepository;
        private readonly IimageServices _IimageServices;

        public SearchController(IElasticSearchRepository elasticSearchRepository, IimageServices iimageServices)
        {
            _elasticSearchRepository = elasticSearchRepository;
            _IimageServices = iimageServices;
        }
        [HttpPost("searchDebounce")]
        public async Task<object> searchDebounce([FromBody] SearchRequest request)
        {
            var suggestions = _elasticSearchRepository.searchDebounce<OnlySources>(s => s
                .Index("only_sources_v2")
                .Suggest(su => su
                    .Completion("my_suggestion", c => c
                        .Field("Title")
                        .Prefix(request.Query)
                        .Fuzzy(f => f
                            .Fuzziness(Fuzziness.Auto)
                        )
                    )
                )
            );
            var groupBySuggestion = GroupBy(suggestions);
            return groupBySuggestion;
        }
        [HttpPost("filter")]
        public async Task<object> Filter([FromBody] SearchQuery request)
        {
            var response = _elasticSearchRepository.Filter<OnlySources>(s => s
                .Index("only_sources_v2")
                .Query(q => q
                    .Bool(b => b
                        .Must(m => m
                            .Match(ma => ma
                                .Field(f => f.Title)
                                .Query(request.TopicName)
                            ),
                            m => m
                            .Match(ma => ma
                                .Field(f => f.Description)
                                .Query(request.SubTopicName)
                            ),
                            m => m
                            .Range(r => r
                                .Field(f => f.Rating)
                                .GreaterThanOrEquals(request.RatingGte)
                                .LessThanOrEquals(request.RatingLte)
                            ),
                            m => m
                            .Range(r => r
                                .Field(f => f.Price)
                                .GreaterThanOrEquals(request.PriceGte)
                                .LessThanOrEquals(request.PricedLte)
                            )
                        )
                    )
                )
            );
            var result = GroupBy(response);
            return result;
        }
        // Models/SearchRequest.cs
        [HttpPost("searchfulltext")]
        public async Task<object> SearchFullText2([FromBody] SearchRequest request)
        {

            var response = _elasticSearchRepository.Filter<OnlySources>(s => s
            .Index("only_sources_v2")
            .Query(q => q
                .MultiMatch(ma => ma
                .Query(request.Query)
                .Fields(f => f
                    .Field(p => p.Title.Suffix("text"), boost: 2.0)
                    .Field(p => p.Description)
                )
            )
            )
        );
            var result = GroupBy(response);
            return result;
        }
        [HttpGet("getDataHome")]
        public async Task<object> getDataHome()
        {
            var response = _elasticSearchRepository.Filter<OnlySources>(s => s.Index("only_sources_v2").Query(q => q.MatchAll()));
            Random random = new Random();
            var result = response.Select(s => new
            {
                imageThumbnail = s.Thumbnail == ""? "https://www.invert.vn/media/uploads/uploads/2022/12/03193534-2-anh-gai-xinh-diu-dang.jpeg" : _IimageServices.GetFile(s.Thumbnail),
                topic = s.TopicName,
                title = s.Title.Input.FirstOrDefault(),
                rating = s.Rating,
                views = random.Next(10, 1000),
                price = s.Price,
                id = s.Id,
                topicID = s.TopicId,
            });
            return result;
        }
        private List<GroupedTopic> GroupBy(List<OnlySources> onlySources)
        {
            var result = onlySources.GroupBy(x => new { x.TopicId, x.TopicName })
                .Select(g => new GroupedTopic
                {
                    TopicId = g.Key.TopicId,
                    TopicName = g.Key.TopicName,
                    SubTopic = g.GroupBy(x => new { x.SubTopicId, x.SubTopicName }).Select(g2 => new GroupedSubTopic
                    {
                        SubTopicId = g2.Key.SubTopicId,
                        SubTopicName = g2.Key.SubTopicName,
                        Sources = g2.Select(x => new GroupedResponse
                        {
                            Id = x.Id,
                            Title = x.Title,
                            Description = x.Description,
                            Thumbnail = x.Thumbnail == null ? "" : _IimageServices.GetFile(x.Thumbnail),
                            Slug = x.Slug,
                            Status = x.Status,
                            Benefit = x.Benefit,
                            Video_intro = x.Video_intro == null ? "" : _IimageServices.GetFile(x.Video_intro),
                            Price = x.Price,
                            Rating = x.Rating,
                            UserId = x.UserId
                        }).ToList()
                    }).ToList()
                }).ToList();
            return result;
        }

        public class SearchRequest
        {
            public string Query { get; set; }
        }
        public class GroupedSubTopic
        {
            public long SubTopicId { get; set; }
            public string SubTopicName { get; set; }
            public List<GroupedResponse> Sources { get; set; }
        }

        public class GroupedTopic
        {
            public long TopicId { get; set; }
            public string TopicName { get; set; }
            public List<GroupedSubTopic> SubTopic { get; set; }
        }
        public class GroupedResponse
        {
            public int Id { get; set; }
            public CompletionField Title { get; set; }
            public string Description { get; set; }
            public string Thumbnail { get; set; }
            public string Slug { get; set; }
            public int? Status { get; set; }
            public string? Benefit { get; set; }
            public string? Video_intro { get; set; }
            public double? Price { get; set; }
            public string Rating { get; set; }
            public int UserId { get; set; }
        }

    }
}

