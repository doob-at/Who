

[CmdletBinding()]
param (
    [Parameter()]
    [string]$Name
)

$Name = "$($Name)_$((get-date).ToString("yyyy-MM-ddTHH-mm-ss"))"

dotnet ef migrations script `
    --context AuthDbContext `
    --project src\Authentication\who.Auth.Sqlite `
    --startup-project src\Authentication\who.Auth.MigrationBuilder `
    --configuration sqlite `
    --output "DbScripts\Sqlite_$($Name).sql" `

dotnet ef migrations script `
    --context AuthDbContext `
    --project src\Authentication\who.Auth.Postgres `
    --startup-project src\Authentication\who.Auth.MigrationBuilder `
    --configuration postgres `
    --output "DbScripts\Postgres_$($Name).sql" `
    --idempotent

dotnet ef migrations script `
    --context AuthDbContext `
    --project src\Authentication\who.Auth.SqlServer `
    --startup-project src\Authentication\who.Auth.MigrationBuilder `
    --configuration sqlserver `
    --output "DbScripts\SqlServer_$($Name).sql" `
    --idempotent
