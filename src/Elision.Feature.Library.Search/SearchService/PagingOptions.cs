namespace Elision.Feature.Library.Search
{
    public class PagingOptions
    {
        public readonly int Offset;
        public readonly int Limit;

        private PagingOptions(int offset, int limit)
        {
            Limit = limit;
            Offset = offset;
        }

        public static readonly PagingOptions None = new PagingOptions(0, int.MaxValue);

        public static PagingOptions FromOffset(int offset, int limit)
        {
            return new PagingOptions(offset, limit);
        }

        public static PagingOptions FromPage(int pageNumber, int pageSize)
        {
            return new PagingOptions((pageNumber - 1) * pageSize, pageSize);
        }
    }
}