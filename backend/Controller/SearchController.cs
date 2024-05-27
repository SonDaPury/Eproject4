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
        public int? PricedLte { get; set; }
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
        public async Task<object> SearchQuery([FromBody] SearchQuery searchquery)
        {
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
                                    )
                                )
                                .Should(m => m.Nested(nn => nn.Path("subTopics.sources").Query(q => q.Bool(b => b.Should(s => s.Range(c => c.Name("subTopics.sources.Rating").GreaterThanOrEquals(searchquery.RatingGte)
                                                                    .LessThanOrEquals(searchquery.RatingLte))))))
                            /*    .Should(
                                    sh => sh
                                        .Nested(nn => nn
                                            .Path("subTopics.sources")
                                            .Query(sq => sq
                                                .Bool(sb => sb
                                                    .Should(sb1 =>
                                                    {
                                                        var shouldQueries = new List<Func<QueryContainerDescriptor<object>, QueryContainer>>();
                                                        if (searchquery.RatingGte != 0 && searchquery.RatingLte != 0)
                                                        {
                                                            shouldQueries.Add(qd => qd
                                                                .Range(r => r
                                                                    .Field("subTopics.sources.Rating")
                                                                    .GreaterThanOrEquals(searchquery.RatingGte)
                                                                    .LessThanOrEquals(searchquery.RatingLte)
                                                                )
                                                            );
                                                        }
                                                        if (searchquery.PriceGte != 0 && searchquery.PricedLte != 0)
                                                        {
                                                            shouldQueries.Add(qd => qd
                                                                .Range(r => r
                                                                    .Field("subTopics.sources.Price")
                                                                    .GreaterThanOrEquals(searchquery.PriceGte)
                                                                    .LessThanOrEquals(searchquery.PricedLte)
                                                                )
                                                            );
                                                        }
                                                        if (shouldQueries.Count > 0)
                                                        {
                                                            return sb1.Bool(nb => nb.Should(shouldQueries.ToArray()));
                                                        }
                                                        else
                                                        {
                                                            return null;
                                                        }
                                                    }
                                                   )
                                                )
                                            )
                                        )
                                )
                                .MinimumShouldMatch(1)*/
                            )
                        )
                    )
            )
        )
    )));
            return result;
        }
    }
}
