using System.Xml;
using Elision.Foundation.Kernel;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;

namespace Elision.Search.ComputedFields
{
    public class ImageUrl : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public string RefFieldName { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }

        public ImageUrl(){}
        public ImageUrl(XmlNode node)
        {
            if (node?.Attributes == null)
                return;

            var attrib = node.Attributes["refFieldName"];
            if (!string.IsNullOrWhiteSpace(attrib?.Value))
                RefFieldName = attrib.Value;

            int parsedInt;
            attrib = node.Attributes["width"];
            if (!string.IsNullOrWhiteSpace(attrib?.Value) && int.TryParse(attrib.Value, out parsedInt))
                Width = parsedInt;

            attrib = node.Attributes["height"];
            if (!string.IsNullOrWhiteSpace(attrib?.Value) && int.TryParse(attrib.Value, out parsedInt))
                Height = parsedInt;
        }

        public virtual object ComputeFieldValue(IIndexable indexable)
        {
            var item = (Item)(indexable as SitecoreIndexableItem);
            return item?.ImageUrl(string.IsNullOrWhiteSpace(RefFieldName) ? FieldName : RefFieldName, Width, Height);
        }
    }
}
