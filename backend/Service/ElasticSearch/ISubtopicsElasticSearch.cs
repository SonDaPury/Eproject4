using backend.Dtos;
using backend.Entities;

namespace backend.Service.ElasticSearch
{
    public interface ISubtopicsElasticSearch
    {
        bool AddSources(SubTopicDto subTopic);
        bool RemoveSources(string topicId, string subTopicId);
        bool UpdateData(SubTopicDto subTopic);
    }
}
