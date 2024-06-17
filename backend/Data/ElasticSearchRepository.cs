using backend.Dtos;
using backend.Entities;
using Nest;

namespace backend.Data
{
    public class SearchResult
    {
        public SourcesElasticSearch Source { get; set; }
        public string TopicName { get; set; }
        public int TopicId { get; set; }
        public string SubTopicName { get; set; }
        public int SubTopicId { get; set; }
    }

    public class ElasticSearchRepository : IElasticSearchRepository
    {
        private IElasticClient _client;
        public ElasticSearchRepository(IElasticClient client)
        {
            _client = client;
        }
        public bool AddorUpdateData<T>(T document, string id) where T : class
        {
            try
            {
                var indexResponse = _client.Index(document, i => i
              .Index("sources_index")
              .Id(id));
                return indexResponse.IsValid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool AddorUpdateDataSources<T>(T document, string id) where T : class
        {
            try
            {
                var indexResponse = _client.Index(document, i => i
              .Index("only_sources")
              .Id(id));
                return indexResponse.IsValid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<T> GetData<T>(Func<SearchDescriptor<T>, ISearchRequest> selector) where T : class
        {
            var list = new List<T>();
            try
            {
                var response = _client.Search<T>(selector);
                if (response.IsValid)
                {
                    var hits = response.Hits;
                    foreach (var hit in hits)
                    {
                        var innerHits = hit.InnerHits;
                        if (innerHits.ContainsKey("filtered_subTopics"))
                        {
                            var filteredSubTopicsHits = innerHits["filtered_subTopics"].Hits;
                            foreach (var subTopicHit in filteredSubTopicsHits.Hits)
                            {
                                if (subTopicHit.InnerHits.ContainsKey("filtered_sources"))
                                {
                                    var subTopic = subTopicHit.Source.As<SubTopcElasticSearch>();
                                    var filteredSourcesHits = subTopicHit.InnerHits["filtered_sources"].Hits;
                                    for (int i = 0; i < filteredSourcesHits.Hits.Count; i++)
                                    {
                                        var sourceHit = filteredSourcesHits.Hits[i];
                                        var source = sourceHit.Source.As<T>();
                                        list.Add(source);
                                    }
                                }
                            }
                        }
                    }
                    return list;
                }
                else
                {
                    Console.WriteLine($"Search failed: {response.ServerError?.Error?.Reason ?? "Unknown error"}");
                    return new List<T>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return new List<T>();
            }
        }
        public List<SearchResult> GetDataSearch<T>(Func<SearchDescriptor<TopicElasticSearch>, ISearchRequest> selector) where T : class
        {
            var list = new List<SearchResult>();
            try
            {
                var response = _client.Search<TopicElasticSearch>(selector);
                if (response.IsValid)
                {
                    TopicElasticSearch topic = new TopicElasticSearch();
                    var hits = response.Hits;
                    foreach (var hit in hits)
                    {
                        topic = hit.Source;
                        var innerHits = hit.InnerHits;
                        if (innerHits.ContainsKey("filtered_subTopics"))
                        {
                            var filteredSubTopicsHits = innerHits["filtered_subTopics"].Hits;
                            foreach (var subTopicHit in filteredSubTopicsHits.Hits)
                            {
                                if (subTopicHit.InnerHits.ContainsKey("filtered_sources"))
                                {
                                    var subTopic = subTopicHit.Source.As<SubTopcElasticSearch>();
                                    var filteredSourcesHits = subTopicHit.InnerHits["filtered_sources"].Hits;
                                    for (int i = 0; i < filteredSourcesHits.Hits.Count; i++)
                                    {
                                        var sourceHit = filteredSourcesHits.Hits[i];
                                        var source = sourceHit.Source.As<SourcesElasticSearch>();
                                        list.Add(new SearchResult
                                        {
                                            Source = source,
                                            TopicName = topic.TopicName,
                                            TopicId = topic.TopicId,
                                            SubTopicName = subTopic.SubTopicName,
                                            SubTopicId = subTopic.SubTopicId
                                        });
                                    }
                                }
                            }
                        }
                    }
                    return list;
                }
                else
                {
                    Console.WriteLine($"Search failed: {response.ServerError?.Error?.Reason ?? "Unknown error"}");
                    return new List<SearchResult>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return new List<SearchResult>();
            }
        }

        public bool RemoveDocument(string id, string index = "sources_index")
        {
            var response = _client.Delete(new DeleteRequest(index, id));
            return response.IsValid;
        }

        public bool UpdateScript(string id, Func<UpdateDescriptor<object, object>, IUpdateRequest<object, object>> selector)
        {
            try
            {
                var response = _client.Update<object>(id, selector);
                if (response.IsValid)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
