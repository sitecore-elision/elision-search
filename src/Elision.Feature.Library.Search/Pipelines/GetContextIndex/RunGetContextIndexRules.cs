using Elision.Feature.Library.Search.Rules;
using Elision.Foundation.Kernel.Diagnostics;
using Elision.Foundation.Rules;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Pipelines.GetContextIndex;
using Sitecore.Data.Items;

namespace Elision.Feature.Library.Search.Pipelines.GetContextIndex
{
    public class RunGetContextIndexRules : GetContextIndexProcessor
    {
        private readonly IRulesRunner _runner;

        public RunGetContextIndexRules(IRulesRunner runner)
        {
            _runner = runner;
        }

        public override void Process(GetContextIndexArgs args)
        {
            using (new TraceOperation("Run GetContextIndex rules"))
            {
                var indexItem = (Item)(SitecoreIndexableItem) args.Indexable;
                _runner.RunGlobalRules("Get Context Index",
                                       indexItem == null ? Sitecore.Context.Database : indexItem.Database,
                                       new GetContextIndexRuleContext(args));
            }
        }
    }
}