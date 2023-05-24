$json = Get-Content -Path .\appsettings.json | ConvertFrom-Json

function SetSecrets ($path, $obj) {
    foreach ($prop in $obj.PSObject.Properties) {
        $key = "$path:$($prop.Name)"
        if ($prop.Value -is [PSCustomObject]) {
            SetSecrets $key $prop.Value
        } else {
            dotnet user-secrets set $key $prop.Value
        }
    }
}

SetSecrets "" $json
