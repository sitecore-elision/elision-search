using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;

namespace Elision.Feature.Library.Search.ComputedFields
{
    public class TemplateDisplayName: IComputedIndexField
    {
        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexItem = (Item)(indexable as SitecoreIndexableItem);
            return indexItem?.Template.DisplayName;
        }

        public string FieldName { get; set; }
        public string ReturnType { get; set; }
    }
}
