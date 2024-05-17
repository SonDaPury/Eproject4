using Nest;

namespace backend.Data
{
    public interface IElasticSearchRepository
    {
        bool AddData<T>(T document, string key) where T : class;
        T GetData<T>(string id, string key) where T : class;
        IEnumerable<T> SearchData<T>(Func<SearchDescriptor<T>, ISearchRequest> selector) where T : class;

    }
}
