using backend.Dtos;
using Nest;

namespace backend.Data
{
    public interface IElasticSearchRepository
    {
        bool AddorUpdateData<T>(T document, string id) where T : class;
        bool AddorUpdateDataSources<T>(T document, string id) where T : class;
        List<T> GetData<T>(Func<SearchDescriptor<T>, ISearchRequest> selector) where T : class;
        bool RemoveDocument(string id, string index = "sources_index");
        bool UpdateScript(string id, Func<UpdateDescriptor<object, object>, IUpdateRequest<object, object>> selector);
        List<SearchResult> GetDataSearch<T>(Func<SearchDescriptor<TopicElasticSearch>, ISearchRequest> selector) where T : class;
    }
}
