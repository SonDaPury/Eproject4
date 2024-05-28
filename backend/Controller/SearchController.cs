using backend.Data;
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
        public int? PricedLte { get; set; } = int.MaxValue;
        public int PaginationIndex { get; set; } = 1;
        public int PaginationSize { get; set; } = 10;
    }
    [Route("api/[controller]")]
    public class SearchController
    {
        private readonly IElasticSearchRepository _elasticSearchRepository;
        public SearchController(IElasticSearchRepository elasticSearchRepository)
        {
            _elasticSearchRepository = elasticSearchRepository;
        }
        [HttpPost]
        public async Task<object> SearchFilter([FromBody] SearchQuery searchquery)
        {
            int PageIndex = (searchquery.PaginationIndex - 1) * searchquery.PaginationSize;
            var result = _elasticSearchRepository.GetData<object>(s => s
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

            return result;
        }
        [HttpPost("searchfulltext")]
        public async Task<object> SearchFullText(string search)
        {
            var result = _elasticSearchRepository.GetData<object>(s => s
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
            return result;
        }
    }
}
