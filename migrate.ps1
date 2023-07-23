param ([string]$name)
cd GameSync.Api.Persistence
dotnet ef --startup-project ../GameSync.Api/ migrations add Test
cd ..