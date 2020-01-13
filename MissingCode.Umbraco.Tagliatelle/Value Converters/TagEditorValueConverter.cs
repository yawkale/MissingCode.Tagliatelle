using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web;

namespace MissingCode.Umbraco.Tagliatelle
{
    public class TagliatelleTagEditorPropertyConverter : PropertyValueConverterBase
    {

        public override bool IsConverter(IPublishedPropertyType propertyType)
        {
            return propertyType.Alias.Equals("tagliatelle.tagEditor");
          
        }

        public override object ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object source, bool preview)
        {
            var nodeIds =
                 source.ToString()
                 .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                 .Select(int.Parse)
                 .ToArray();

            return nodeIds;
        }

        public override object ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
        {
            // Check if the source value has been populated. If not, return null
            if (inter == null)
            {
                return null;
            }

            // Convert the source value to an array of integars 
            int[] nodeIds = (int[])inter;
            List<IPublishedContent> tagliatelleTagEditor = new List<IPublishedContent>();
            bool dynamicInvocation = false;
            return null;
            

         

            //if (UmbracoContext.Current != null)
            //{
            //    UmbracoHelper umbHelper = new UmbracoHelper(UmbracoContext.Current);

            //    if (nodeIds.Length > 0)
            //    {

            //        var objectType = UmbracoObjectTypes.Unknown;

            //        foreach (var nodeId in nodeIds)
            //        {
            //            var tag = GetPublishedContent(nodeId, ref objectType, UmbracoObjectTypes.Document, umbHelper.TypedContent)
            //                        ?? GetPublishedContent(nodeId, ref objectType, UmbracoObjectTypes.Media, umbHelper.TypedMedia)
            //                        ?? GetPublishedContent(nodeId, ref objectType, UmbracoObjectTypes.Member, umbHelper.TypedMember);

            //            if (tag != null)
            //            {
            //                tagliatelleTagEditor.Add(dynamicInvocation ? tag.AsDynamic() : tag);
            //            }
            //        }

            //    }

            //    return dynamicInvocation
            //               ? new DynamicPublishedContentList(tagliatelleTagEditor.Where(x => x != null))
            //               : tagliatelleTagEditor.Where(x => x != null);

            //}
            //    else
            //    {
            //        return null;
            //    }
        }

        
        

       
        //public Type GetPropertyValueType(PublishedPropertyType propertyType)
        //{
        //    return typeof(IEnumerable<IPublishedContent>);
        //}

       
        //private IPublishedContent GetPublishedContent(int nodeId, ref UmbracoObjectTypes actualType, UmbracoObjectTypes expectedType, Func<int, IPublishedContent> contentFetcher)
        //{
        //    // is the actual type supported by the content fetcher?
        //    if (actualType != UmbracoObjectTypes.Unknown && actualType != expectedType)
        //    {
        //        // no, return null
        //        return null;
        //    }

        //    // attempt to get the content
        //    var content = contentFetcher(nodeId);
        //    if (content != null)
        //    {
        //        // if we found the content, assign the expected type to the actual type so we don't have to keep looking for other types of content
        //        actualType = expectedType;
        //    }
        //    return content;
        //}

    }
}
