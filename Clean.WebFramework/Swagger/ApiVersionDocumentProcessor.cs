using Clean.WebFramework.Extensions;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace Clean.WebFramework.Swagger;

public  class ApiVersionDocumentProcessor : IDocumentProcessor
{
    public void Process(DocumentProcessorContext context)
    {
       var version = context.Document.Info.Version;

        var pathsToRemove = context.Document.Paths.Where(w => !RegExHelpers.MarchesApiVersion(version,w.Key))
            .Select(s => s.Key).ToList();

        foreach (var pathToRemove in pathsToRemove)
        {
            context.Document.Paths.Remove(pathToRemove);
        }
    }
}
