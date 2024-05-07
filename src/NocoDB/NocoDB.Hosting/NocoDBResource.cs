// For ease of discovery, resource types should be placed in
// the Aspire.Hosting.ApplicationModel namespace. If there is
// likelihood of a conflict on the resource name consider using
// an alternative namespace.
namespace Aspire.Hosting.ApplicationModel;

public sealed class NocoDBResource(string name) : ContainerResource(name)
{
    internal const string HttpEndpointName = "http";
}