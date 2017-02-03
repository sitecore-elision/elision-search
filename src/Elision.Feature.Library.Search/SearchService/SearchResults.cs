using System.Collections.Generic;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;

namespace Elision.Feature.Library.Search
{
    public class SearchResults<T> where T : ISearchResult
    {
        public T[] Documents { get; set; }
        public int TotalSearchResults { get; set; }
        public List<FacetCategory> FacetCategories { get; set; }
    }
}