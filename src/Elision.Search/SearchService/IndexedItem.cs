using System.Collections.Generic;
using System.ComponentModel;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Converters;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;

namespace Elision.Search
{
    public class IndexedItem : SearchResultItem
    {
        [IndexField("_haspresentation")]
        public bool HasPresentation { get; set; }

        [IndexField("hidefromsearchresults")]
        public bool HideFromSearchResults { get; set; }

        [IndexField("_latestversion")]
        public bool IsLatestVersion { get; set; }

        [TypeConverter(typeof(IndexFieldEnumerableConverter))]
        [IndexField("_basetemplates")]
        public IEnumerable<ID> BaseTemplates { get; set; }
    }
}