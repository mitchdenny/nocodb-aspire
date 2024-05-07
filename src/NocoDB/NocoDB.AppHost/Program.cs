var builder = DistributedApplication.CreateBuilder(args);

var postgresUsername = builder.AddParameter("pgusername");
var postgresPassword = builder.AddParameter("pgpassword", secret: true);

var postgresServer = builder.AddPostgres("pgsql", postgresUsername, postgresPassword).WithDataVolume().WithPgAdmin();
var postgresDatabase = postgresServer.AddDatabase("db1");

var nocodb = builder.AddNocoDB("nocodb", port: 8088)
                    .WithReference(postgresDatabase);

builder.AddProject<Projects.NocoDB_SampleApi>("nocodb-sampleapi")
       .WithReference(nocodb.GetEndpoint("http"));

builder.Build().Run();
