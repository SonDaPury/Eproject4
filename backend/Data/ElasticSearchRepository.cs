using backend.Dtos;
using Nest;

namespace backend.Data
{
    public class ElasticSearchRepository : IElasticSearchRepository
    {
        private IElasticClient _client;
        public ElasticSearchRepository(IElasticClient client)
        {
            _client = client;
        }
        public bool AddorUpdateData<T>(T document, string index, string id) where T : class
        {
            var indexResponse = _client.Index(document, i => i
    .Index("sources_index")
    .Id(id)
);
            return indexResponse.IsValid;
        }

        public bool DeleIndex(string index)
        {
            throw new NotImplementedException();
        }

        public List<T> GetData<T>(Func<SearchDescriptor<T>, ISearchRequest> selector) where T : class
        {
            var response = _client.Search<T>(selector);
            return response.Documents.ToList();
        }

        public bool RemoveDocument(string index, string id)
        {
            var response = _client.Delete(new DeleteRequest(index, id));
            return response.IsValid;
        }

        public bool RemoveChildrenDocumentByScript()
        {
            throw new NotImplementedException();
        }

        public bool AddData<T>(T document, string key) where T : class
        {
            throw new NotImplementedException();
        }

        public T GetData<T>(string id, string key) where T : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> SearchData<T>(Func<SearchDescriptor<T>, ISearchRequest> selector) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
