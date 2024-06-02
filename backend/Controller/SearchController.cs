using backend.Data;
using backend.Dtos;
using backend.Service.Interface;
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
        public SearchController(IElasticSearchRepository elasticSearchRepository, IimageServices iimageServices)
        {
            _elasticSearchRepository = elasticSearchRepository;
            _IimageServices = iimageServices;
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
        [HttpPost("searchfulltext")]
        public async Task<object> SearchFullText(string search)
        {
            var result = _elasticSearchRepository.GetData<SourcesElasticSearch>(s => s
            .Index("sources_index")
            .From(0)
            .Size(10)
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
    }
}
