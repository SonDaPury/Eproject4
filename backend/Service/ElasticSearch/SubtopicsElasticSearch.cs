using backend.Data;
using backend.Dtos;
using backend.Entities;

namespace backend.Service.ElasticSearch
{
    public class SubtopicsElasticSearch : ISubtopicsElasticSearch
    {
        private readonly IElasticSearchRepository elasticSearchRepository;
        public SubtopicsElasticSearch(IElasticSearchRepository _elasticSearchRepository)
        {
            elasticSearchRepository = _elasticSearchRepository;
        }
        public bool AddSources(SubTopicDto subTopic)
        {
            var addResponse = elasticSearchRepository.UpdateData(subTopic.TopicId.ToString(), u => u
                .Index("sources_index")
                .Script(s => s
                    .Source("ctx._source.subTopics.add(params.subtopic)")
                    .Params(p => p.Add("subtopic", subTopic))
                )
            );
            return addResponse;
        }

        public bool RemoveSources(string topicId, string subTopicId)
        {
            var removeResponse = elasticSearchRepository.UpdateData(topicId, u => u
                .Index("sources_index")
                .Script(s => s
                    .Source(@"
                for (int i = 0; i < ctx._source.subTopics.size(); i++) {
                    if (ctx._source.subTopics[i].SubTopicId == params.subTopicId) {
                        ctx._source.subTopics.remove(i);
                        break;
                    }
                }")
                    .Params(p => p.Add("subTopicId", subTopicId))
                )
            );
            return removeResponse;
        }

        public bool UpdateData(SubTopicDto subTopic)
        {
            var updateResponse = elasticSearchRepository.UpdateData(subTopic.TopicId.ToString(), u => u
                .Index("sources_index")
                .Script(s => s
                    .Source(@"
                for (int i = 0; i < ctx._source.subTopics.size(); i++) {
                    if (ctx._source.subTopics[i].SubTopicId == params.subTopicId) {
                        ctx._source.subTopics[i].SubTopicName = params.subTopicName;
                        break;
                    }
                }")
                    .Params(p => p
                        .Add("subTopicId", subTopic.Id)
                        .Add("subTopicName", subTopic.SubTopicName)
                    )
                )
            );
            return updateResponse;
        }

    }
}
