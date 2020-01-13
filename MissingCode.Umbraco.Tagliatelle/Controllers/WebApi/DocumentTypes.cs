using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace MissingCode.Umbraco.Tagliatelle
{
    [PluginController("Tagliatelle")]
    public class DocumentTypesApiController : UmbracoAuthorizedApiController
    {
        

        public List<DocumentType> GetDocumentTypes()
        {
            var ct = Services.ContentTypeService;
            var documentTypes = new List<DocumentType>();

            IEnumerable<IContentType> contentTypes = ct.GetAll();
            foreach (IContentType contentType in contentTypes)
            {
                var documentType = new DocumentType
                {
                    Name = contentType.Name,
                    Alias = contentType.Alias
                };
                documentTypes.Add(documentType);
            }

            return documentTypes;

        }
    }

    public class DocumentType
    {
        public string Name { get; set; }
        public string Alias { get; set; }
    }
}
