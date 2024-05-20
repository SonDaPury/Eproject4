using backend.Data;
using backend.Dtos;
using backend.Entities;

namespace backend.Service.ElasticSearch
{
    public class TopicsElasticSearch : ITopicsElasticSearch
    {
        private readonly IElasticSearchRepository elasticSearchRepository;
        public TopicsElasticSearch(IElasticSearchRepository _elasticSearchRepository)
        {
            elasticSearchRepository = _elasticSearchRepository;
        }
        public bool AddTopic(TopicElasticSearch topic)
        {
            var addResponse = elasticSearchRepository.AddorUpdateData<TopicElasticSearch>(topic, "sources_index", topic.TopicId.ToString());
            return addResponse;
        }

        public bool RemoveTopic(string topicId)
        {
            var removeResponse = elasticSearchRepository.RemoveDocument("sources_index", topicId);
            return removeResponse;
        }

        public bool UpdateData(TopicDto topic)
        {
            var UpdateResponse = elasticSearchRepository.UpdateData(topic.Id.ToString(), u => u.Index("sources_index").Script(s =>
            s.Source(@"ctx._source.TopicName = params.topicName").Params(p => p.Add("topicName", topic.TopicName))));
            return UpdateResponse;
        }
    }
}
