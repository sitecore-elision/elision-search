using Sitecore.Rules.Actions;

namespace Elision.Search.Rules
{
    public class UseIndexAction<T> : RuleAction<T> where T : GetContextIndexRuleContext
    {
        protected string IndexName { get; set; }

        public override void Apply(T ruleContext)
        {
            ruleContext.Args.Result = IndexName;
        }
    }
}
