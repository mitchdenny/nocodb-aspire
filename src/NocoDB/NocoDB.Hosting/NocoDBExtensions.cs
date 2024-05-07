using Aspire.Hosting.ApplicationModel;

namespace Aspire.Hosting;

public static class NocoDBResourceBuilderExtensions
{
    public static IResourceBuilder<NocoDBResource> AddNocoDB(this IDistributedApplicationBuilder builder, string name, int? port = null)
    {
        var resource = new NocoDBResource(name);

        return builder.AddResource(resource)
                      .WithImage(NocoDBContainerImageTags.Image)
                      .WithImageRegistry(NocoDBContainerImageTags.Registry)
                      .WithImageTag(NocoDBContainerImageTags.Tag)
                      .WithHttpEndpoint(
                          targetPort: 8080,
                          port: port,
                          name: NocoDBResource.HttpEndpointName);
    }

    public static IResourceBuilder<NocoDBResource> WithReference(this IResourceBuilder<NocoDBResource> builder, IResourceBuilder<PostgresDatabaseResource> postgresDatabase)
    {
        var postgresServerBuilder = builder.ApplicationBuilder.CreateResourceBuilder(postgresDatabase.Resource.Parent);
        var postgresServerEndpoint = postgresServerBuilder.GetEndpoint("tcp");

        // NOTE: The expression {postgresServerEndpoint.Property(EndpointProperty.Host)} does not work because localhost is
        //       not correctly substituted with host.docker.internal. We need to fix this bug in Aspire.
        var referenceExpression = ReferenceExpression.Create($"pg://{postgresServerEndpoint.Property(EndpointProperty.Host)}:{postgresServerEndpoint.Property(EndpointProperty.Port)}?u={postgresServerBuilder.Resource.UserNameParameter}&p={postgresServerBuilder.Resource.PasswordParameter}&d={postgresDatabase.Resource.DatabaseName}");

        return builder.WithEnvironment("NC_DB", referenceExpression);
    }
}

// This class just contains constant strings that can be updated periodically
// when new versions of the underlying container are released.
internal static class NocoDBContainerImageTags
{
    internal const string Registry = "docker.io";

    internal const string Image = "nocodb/nocodb";

    internal const string Tag = "0.207.0";
}