namespace Elision.Search
{
    public interface ISiteSearcher
    {
        SearchResults<IndexedItem> Search(SearchOptions options);
    }

    public class SiteSearcher : SearchService<IndexedItem, SearchOptions>, ISiteSearcher
    {
    }
}