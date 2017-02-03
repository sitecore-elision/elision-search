using System;
using System.Collections.Generic;
using System.Linq;
using Elision.Foundation.Kernel;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.Utilities;

namespace Elision.Search
{
    public abstract class SearchService<TResult, TOptions>
        where TResult : IndexedItem
        where TOptions : SearchOptions
    {
        protected virtual ISearchIndex GetIndex(TOptions options)
        {
            return ContentSearchManager.GetIndex(new SitecoreIndexableItem(options.ContextPage));
        }

        public virtual SearchResults<TResult> Search(TOptions options)
        {
            var index = GetIndex(options);
            using (var searchContext = index.CreateSearchContext())
            {
                var queryable = BuildQueryable(options, searchContext);
                return BuildResults(queryable, options);
            }
        }

        protected virtual SearchResults<TResult> BuildResults(IQueryable<TResult> queryable, TOptions options)
        {
            var results = ApplyPaging(queryable, options).GetResults();
            var totalResults = results.TotalSearchResults;

            return new SearchResults<TResult>
            {
                Documents = GetReturnDocuments(options, results),
                FacetCategories = results.Facets == null ? new List<FacetCategory>() : results.Facets.Categories,
                TotalSearchResults = totalResults
            };
        }

        protected virtual TResult[] GetReturnDocuments(TOptions options, Sitecore.ContentSearch.Linq.SearchResults<TResult> results)
        {
            return results.Hits.Select(x => x.Document).ToArray();
        }

        protected virtual IQueryable<TResult> BuildQueryable(TOptions options, IProviderSearchContext searchContext)
        {
            var queryable = searchContext.GetQueryable<TResult>();
            queryable = ApplyWebsiteFilter(queryable, options);
            queryable = ApplyTemplateFilter(queryable, options);
            queryable = ApplyHasLayoutFilter(queryable, options);
            queryable = ApplyLanguageFilter(queryable, options);
            queryable = ApplyLatestVersionFilter(queryable, options);
            queryable = ApplyKeywordFilter(queryable, options);
            return queryable;
        }

        protected virtual IQueryable<TResult> ApplyPaging(IQueryable<TResult> queryable, TOptions options)
        {
            if (options.Paging.Offset > 0)
                queryable = queryable.Skip(options.Paging.Offset);
            if (options.Paging.Limit > 0)
                queryable = queryable.Take(options.Paging.Limit);

            return queryable;
        }

        protected virtual IQueryable<TResult> ApplyKeywordFilter(IQueryable<TResult> queryable, TOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.Query))
                return queryable;

            var keywords = options.Query.ToLowerInvariant().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var contentPredicate = PredicateBuilder.True<TResult>();
            contentPredicate = keywords.Aggregate(contentPredicate, (current, keyword) => current.And(x => x.Content.Like(keyword)));

            var namePredicate = PredicateBuilder.True<TResult>();
            namePredicate = keywords.Aggregate(namePredicate, (current, keyword) => current.And(x => x.Name.Like(keyword)));

            return queryable.Where(contentPredicate.Or(namePredicate));
        }

        protected virtual IQueryable<TResult> ApplyWebsiteFilter(IQueryable<TResult> queryable, TOptions options)
        {
            if (options.ContextPage == null)
                return queryable;

            var homeItem = options.ContextPage.GetSite().GetStartItem();
            if (homeItem == null)
                return queryable;

            var websitePredicate = PredicateBuilder.True<TResult>();
            websitePredicate = websitePredicate.And(x => x.Paths.Contains(homeItem.ID));

            return queryable.Where(websitePredicate);
        }

        protected virtual IQueryable<TResult> ApplyTemplateFilter(IQueryable<TResult> queryable, TOptions options)
        {
            if (options.Templates == null || !options.Templates.Any())
                return queryable;

            var templatePredicate = PredicateBuilder.True<TResult>();
            templatePredicate = options.Templates.Aggregate(templatePredicate, (current, templateId) => current.Or(x => x.BaseTemplates.Contains(templateId)));

            return queryable.Where(templatePredicate);
        }

        protected virtual IQueryable<TResult> ApplyHasLayoutFilter(IQueryable<TResult> queryable, TOptions options)
        {
            return queryable.Where(x => x.HasPresentation && !x.HideFromSearchResults);
        }

        protected virtual IQueryable<TResult> ApplyLanguageFilter(IQueryable<TResult> queryable, TOptions options)
        {
            return string.IsNullOrWhiteSpace(options.Language)
                ? queryable
                : queryable.Filter(x => x.Language == options.Language);
        }

        protected virtual IQueryable<TResult> ApplyLatestVersionFilter(IQueryable<TResult> queryable, TOptions options)
        {
            return queryable.Filter(x => x.IsLatestVersion);
        }
    }
}
