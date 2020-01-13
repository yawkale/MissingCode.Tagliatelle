using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace MissingCode.Umbraco.Tagliatelle
{
    [PluginController("Tagliatelle")]
    public class TagEditorApiController : UmbracoAuthorizedApiController
    {


        public IEnumerable<string> GetTags(int currentNodeId, string containerId, string documentTypeAlias)
        {
            //var udi = Udi.Parse(containerId);
            var udi = GuidUdi.Parse(containerId);
            var entitiy = Services.EntityService.Get(udi.Guid);
            var cs = Services.ContentService;
            /* Get the tags container */
            var tagContainer = cs.GetById(entitiy.Id);


            /* Compile a list of all tag pages that exist as children of the tags container */
            var tags = cs.GetPagedChildren(tagContainer.Id, 0, int.MaxValue, out var count).Where(x => x.ContentType.Alias == documentTypeAlias).Select(x => x.Name);
            return tags;
        }

        public IEnumerable<string> GetTagNames(string nodeIds, string documentTypeAlias)
        {
            var cs = Services.ContentService;

            if (string.IsNullOrWhiteSpace(nodeIds))
            {
                return new List<string>();
            }

            var ids = nodeIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
            var nodes = cs.GetByIds(ids);

            var values = nodes.Where(x => x.ContentType.Alias == documentTypeAlias).Select(x => x.Name);

            return values;
        }

        public IEnumerable<int> GetAndEnsureNodeIdsForTags(string currentNodeId, string tags, string containerId, string documentTypeAlias)
        {
            var udi = GuidUdi.Parse(containerId);
            var entitiy = Services.EntityService.Get(udi.Guid);

            var cs = Services.ContentService;
            if (string.IsNullOrWhiteSpace(tags))
            {
                return new List<int>();
            }

            // put posted tags in an array
            var postedTags = tags.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            // get the current node
            var node = cs.GetById(int.Parse(currentNodeId));

            // get all existing tag nodes in container
            var tagContainer = cs.GetById(entitiy.Id);
            var allTagNodes = cs.GetPagedChildren(tagContainer.Id, 0, int.MaxValue, out var count).Where(x => x.ContentType.Alias == documentTypeAlias);

            bool hasNewTags = false;
            foreach (string postedTag in postedTags)
            {
                // get tag names which do not already exist in the tag container
                bool found = allTagNodes.Any(x => x.Name == postedTag);
                if (!found)
                {
                    // tag node doesnt exist so create new node
                    var dic = new Dictionary<string, object>() { { documentTypeAlias, postedTag } };
                    var newTag = cs.CreateContent(postedTag, tagContainer.GetUdi(), documentTypeAlias);
                    cs.SaveAndPublish(newTag);
                    hasNewTags = true;
                }
            }

            // re-get container because new nodes might have been added.
            tagContainer = cs.GetById(entitiy.Id);
            if (hasNewTags)
            {
                // new tag so sort!
                cs.Sort(cs.GetPagedChildren(tagContainer.Id, 0, int.MaxValue, out var count1).OrderBy(x => x.Name));
            }

            // get all tag ids, and return
            var tagIds = cs.GetPagedChildren(tagContainer.Id, 0, int.MaxValue, out var count2).Where(x => postedTags.Contains(x.Name)).Select(x => x.Id);

            return tagIds;
        }
    }
}
