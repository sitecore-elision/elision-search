using System.Collections.Generic;
using Elision.Foundation.Kernel;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Elision.Search
{
    public class SearchOptions
    {
        public readonly PagingOptions Paging;
        public readonly Item ContextPage;
        public readonly string Query;
        public readonly IEnumerable<ID> Templates;
        public readonly string Language;

        public SearchOptions() : this(null) { }
        public SearchOptions(PagingOptions pagingOptions, Item contextPage = null, string query = null, IEnumerable<ID> templates = null, string language = null)
        {
            ContextPage = contextPage ?? Context.Item;
            Paging = pagingOptions ?? PagingOptions.None;
            Query = query;
            Templates = templates ?? new ID[0];
            Language = language.Or(Context.Language.Name);
        }
    }
}