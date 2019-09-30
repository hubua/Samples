# Samples
.NET code samples

Count lines in cs:
`dir -Recurse *.cs | Get-Content | Measure-Object -Line`

Size of cs:
`dir -Recurse *.cs | measure Length -s`
