using backend.Dtos;
using backend.Entities;

namespace backend.Service.ElasticSearch
{
    public interface ISourcesElasticSearch
    {
        bool AddSources(string topicId, SourceDto source);
        bool RemoveSources(string topicId, string subTopicId, string sourceId);
        bool UpdateData(string topicId, SourceDto source);
    }
}
