

[CmdletBinding()]
param (
    [Parameter()]
    [string]$Name
)


$Name = "$($Name)_$((get-date).ToString("yyyy-MM-ddTHH-mm-ss"))"

dotnet ef migrations add $Name `
    --context AuthDbContext `
    --project src\Authentication\who.Auth.Sqlite `
    --startup-project src\Authentication\who.Auth.MigrationBuilder `
    --configuration sqlite 

dotnet ef migrations add $Name `
    --context AuthDbContext `
    --project src\Authentication\who.Auth.Postgres `
    --startup-project src\Authentication\who.Auth.MigrationBuilder `
    --configuration postgres 

dotnet ef migrations add $Name `
    --context AuthDbContext `
    --project src\Authentication\who.Auth.SqlServer `
    --startup-project src\Authentication\who.Auth.MigrationBuilder `
    --configuration sqlserver 

