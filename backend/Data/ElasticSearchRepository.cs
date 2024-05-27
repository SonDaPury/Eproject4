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

        public List<T> GetData<T>(Func<SearchDescriptor<T>, ISearchRequest> selector) where T : class
        {
            try
            {
                var response = _client.Search<T>(selector);
                if (response.IsValid)
                {
                    return response.Documents.ToList();
                }
                else
                {
                    Console.WriteLine($"Search failed: {response.ServerError.Error.Reason}");
                    return new List<T>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return new List<T>();
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
