### Service Discovery (Windows only)

`consul.exe agent -dev`

### Migration

`dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Migrations/PersistedGrantDb`

`dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Migrations/ConfigurationDb`

`dotnet ef migrations add InitDatabase -c AppDbContext -o Migrations/SampleDb`

`dotnet run`

### Login

`root@shoppingcart.com` / `root`
