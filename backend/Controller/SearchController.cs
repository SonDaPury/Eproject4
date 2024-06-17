﻿using backend.Data;
using backend.Dtos;
using backend.Service.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Nest;

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
        private readonly IElasticClient _elasticClient;

        public SearchController(IElasticSearchRepository elasticSearchRepository, IimageServices iimageServices, IElasticClient client)
        {
            _elasticSearchRepository = elasticSearchRepository;
            _IimageServices = iimageServices;
            _elasticClient = client;
        }
        [HttpPost]
        public async Task<object> SearchQuery([FromBody] SearchQuery searchquery)
        {
            var result = _elasticSearchRepository.GetData<SourcesElasticSearch>(s => s
    .Index("sources_index")
    .Query(q => q
        .Bool(b => b
            .Must(m => m
                .Match(ma => ma
                    .Field("TopicName")
                    .Query(searchquery.TopicName)
                ),
                m => m
                    .Nested(n => n
                        .Path("subTopics")
                        .Query(nq => nq
                            .Bool(nb => nb
                                .Must(nm => nm
                                    .Match(nma => nma
                                        .Field("subTopics.SubTopicName")
                                        .Query(searchquery.SubTopicName)
                                           ),
                                           nm => nm
                                        .Nested(nn => nn
                                            .Path("subTopics.sources")
                                                   .Query(nq2 => nq2
                                                       .Bool(nb2 => nb2
                                                           .Must(nmm => nmm
                                                                .Range(r => r
                                                                    .Field("subTopics.sources.Rating")
                                                                    .GreaterThanOrEquals(searchquery.RatingGte)
                                                                    .LessThanOrEquals(searchquery.RatingLte)
                                                               ),
                                                               nmm => nmm
                                                                .Range(r => r
                                                                    .Field("subTopics.sources.Price")
                                                                    .GreaterThanOrEquals(searchquery.PriceGte)
                                                                    .LessThanOrEquals(searchquery.PricedLte)
                                                                )
                                                           )
                                                       )
                                                   )
                                                   .InnerHits(ih => ih
                                                       .Name("filtered_sources")
                                                   )
                                               )
                                       )
                                   )
                               )
                               .InnerHits(ih => ih
                                   .Name("filtered_subTopics")
                               )
                           )
                   )
               )
           )
                                                            );
            var test = result.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Thumbnail = _IimageServices.GetFile(x.Thumbnail),
                Slug = x.Slug,
                Status = x.Status,
                Benefit = x.Benefit,
                Video_intro = _IimageServices.GetFile(x.Video_intro),
                Price = x.Price,
                Rating = x.Rating,
                UserId = x.UserId

            }
             ).ToList();
            return test;
        }
 /*       [HttpPost("searchfulltext")]
        public async Task<object> SearchFullTextTEST(string search)
        {
            var result = _elasticSearchRepository.GetDataSearch<TopicElasticSearch>(s => s
            .Index("sources_index")
            .Query(q => q
                .Nested(n => n
                    .Path("subTopics")
                    .Query(nq => nq
                        .Nested(nn => nn
                            .Path("subTopics.sources")
                            .Query(nsq => nsq
                                .MultiMatch(mm => mm
                                    .Query(search)
                                    .Fields(f => f
                                        .Field("subTopics.sources.Title", boost: 2)
                                        .Field("subTopics.sources.Description")
                                            )
                                        )
                                )
                            .InnerHits(ih => ih
                                .Name("filtered_sources")
                            )
                        )
                    )
                    .InnerHits(ih => ih
                        .Name("filtered_subTopics")
            )
        )
           ));
            result.ForEach(x => x.Source.Thumbnail = _IimageServices.GetFile(x.Source.Thumbnail));
            result.ForEach(x => x.Source.Video_intro = _IimageServices.GetFile(x.Source.Video_intro));

            var test = result.GroupBy(s => new { s.TopicName, s.TopicId }).Select(x => new
            {
                TopicName = x.Key.TopicName,
                TopicId = x.Key.TopicId,
                SubTopics = x.GroupBy(s => new { s.SubTopicName, s.SubTopicId }).Select(y => new
                {
                    SubTopicName = y.Key.SubTopicName,
                    SubTopicId = y.Key.SubTopicId,
                    Sources = y.Select(z => new
                    {
                        Id = z.Source.Id,
                        Title = z.Source.Title,
                        Description = z.Source.Description,
                        Thumbnail = z.Source.Thumbnail,
                        Slug = z.Source.Slug,
                        Status = z.Source.Status,
                        Benefit = z.Source.Benefit,
                        Video_intro = z.Source.Video_intro,
                        Price = z.Source.Price,
                        Rating = z.Source.Rating,
                        UserId = z.Source.UserId
                    }).ToList()
                }).ToList()
            }).ToList();
            return test;
        }*/
        [HttpPost("searchfulltext")]
        public async Task<object> SearchFullText([FromBody] SearchRequest request)
        {
            var response = await _elasticClient.SearchAsync<OnlySources>(s => s
                .Index("only_sources")
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
            var suggestions = response.Suggest["my_suggestion"].SelectMany(s => s.Options).Select(o => o.Source).ToList();
            suggestions.ForEach(x => x.Thumbnail = _IimageServices.GetFile(x.Thumbnail));
            suggestions.ForEach(x => x.Video_intro = _IimageServices.GetFile(x.Video_intro));
            return suggestions;
        }
        // Models/SearchRequest.cs
        public class SearchRequest
        {
            public string Query { get; set; }
        }
    }
}

