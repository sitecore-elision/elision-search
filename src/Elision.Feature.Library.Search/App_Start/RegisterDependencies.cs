using Elision.Foundation.Rules;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Elision.Feature.Library.Search
{
    public class RegisterDependencies : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IRulesRunner, RulesRunner>();
        }
    }
}