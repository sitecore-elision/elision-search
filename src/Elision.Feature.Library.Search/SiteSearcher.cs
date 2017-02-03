namespace Elision.Feature.Library.Search
{
    public interface ISiteSearcher
    {
        SearchResults<IndexedItem> Search(SearchOptions options);
    }

    public class SiteSearcher : SearchService<IndexedItem, SearchOptions>, ISiteSearcher
    {
    }
}