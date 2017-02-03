using Elision.Foundation.Rules;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Pipelines.GetContextIndex;

namespace Elision.Search.Rules
{
    public class GetContextIndexRuleContext : EnhancedRuleContext
    {
        public readonly GetContextIndexArgs Args;

        public GetContextIndexRuleContext(GetContextIndexArgs args)
        {
            Args = args;
            Item = (SitecoreIndexableItem) args.Indexable;
        }
    }
}
