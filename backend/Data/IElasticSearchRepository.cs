using Nest;

namespace backend.Data
{
    public interface IElasticSearchRepository
    {
        bool AddorUpdateData<T>(T document, string index, string id) where T : class;
        List<T> GetData<T>(Func<SearchDescriptor<T>, ISearchRequest> selector) where T : class;
        bool DeleIndex(string index);
        bool RemoveDocument(string index, string id);
        bool RemoveChildrenDocumentByScript();
        bool UpdateData(string Id, Func<UpdateDescriptor<object, object>, IUpdateRequest<object, object>> selector);
    }
}
