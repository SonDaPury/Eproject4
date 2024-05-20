using backend.Dtos;
using backend.Entities;

namespace backend.Service.ElasticSearch
{
    public interface ITopicsElasticSearch
    {
        bool AddTopic(TopicElasticSearch topic);
        bool RemoveTopic(string topicId);
        bool UpdateData(TopicDto topic);
    }
}
