using System;
using System.Linq;
using System.Xml;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Comparers;
using Sitecore.Data.Items;

namespace Elision.Search.ComputedFields
{
    public class LinkedItemField : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public string RefFieldName { get; set; }

        public string LinkedItemFieldName { get; set; }
        public LinkedItemOrderBy Order { get; set; }
        public bool OrderReversed { get; set; }
        public string Separator { get; set; }

        public LinkedItemField() { }
        public LinkedItemField(XmlNode node)
        {
            if (node?.Attributes == null)
                return;

            var attrib = node.Attributes["refFieldName"];
            if (!string.IsNullOrWhiteSpace(attrib?.Value))
                RefFieldName = attrib.Value;

            attrib = node.Attributes["linkedItemFieldName"];
            if (!string.IsNullOrWhiteSpace(attrib?.Value))
                LinkedItemFieldName = attrib.Value;

            LinkedItemOrderBy parsedOrder;
            attrib = node.Attributes["order"];
            if (!Enum.TryParse(attrib?.Value, true, out parsedOrder))
                Order = LinkedItemOrderBy.FieldValueOrder;

            bool parsedBool;
            attrib = node.Attributes["orderReversed"];
            if (!string.IsNullOrWhiteSpace(attrib?.Value) && bool.TryParse(attrib.Value, out parsedBool))
                OrderReversed = parsedBool;

            attrib = node.Attributes["separator"];
            if (!string.IsNullOrWhiteSpace(attrib?.Value))
                Separator = attrib.Value;
        }

        public virtual object ComputeFieldValue(IIndexable indexable)
        {
            var sitecoreIndexableItem = ((SitecoreIndexableItem) indexable);
            if (sitecoreIndexableItem?.Item == null)
                return null;

            var linkedItems = sitecoreIndexableItem.Item.GetLinkedItems(RefFieldName.Or(FieldName));

            switch (Order)
            {
                case LinkedItemOrderBy.CmsOrder:
                    linkedItems = linkedItems.OrderBy(x => x, new ItemComparer());
                    break;
                case LinkedItemOrderBy.NameAlpha:
                    linkedItems = linkedItems.OrderBy(x => string.IsNullOrWhiteSpace(x.DisplayName) ? x.Name : x.DisplayName);
                    break;
            }
            if (OrderReversed)
                linkedItems = linkedItems.Reverse();

            Func<Item, string> selector;
            if (string.IsNullOrWhiteSpace(LinkedItemFieldName) || LinkedItemFieldName.ToLowerInvariant() == "displayname")
                selector = x => string.IsNullOrWhiteSpace(x.DisplayName) ? x.Name : x.DisplayName;
            else if (LinkedItemFieldName.ToLowerInvariant() == "name")
                selector = x => x.Name;
            else
                selector = x => x[LinkedItemFieldName];

            return string.Join(Separator ?? "|", linkedItems.Select(selector));
        }
    }

    public enum LinkedItemOrderBy
    {
        FieldValueOrder,
        CmsOrder,
        NameAlpha
    }
}
