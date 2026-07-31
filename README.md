
# CEPLAS validation packages

- Submit here: 
  - https://github.com/nfdi4plants/arc-validate-package-registry
  - https://avpr.nfdi4plants.org

## Design

- incremental, but independent?
  - while logically building on one another (and at least in part mutually exclusive), every package should be useful for itself 

- ceplas-01-investigation/
- ceplas-02-biometadata/
- ceplas-03-biodata/
- 


## Notes

A Tiny bit of convention to make writing the description easier... 

Comment test cases like this

```fsharp
// TestCase Critical: Investigation contains contact
// TestCase Non-critical: At least one investigation contact should have role 'principal investigator'
```

to simply grep them

```bash
{
  echo "\t## Critical quality criteria"
  grep -h "TestCase Critical" *.fsx | sed 's|^[[:space:]]*// TestCase Critical: |\t- |'
  echo
  echo "\t## Non-critical quality criteria"
  grep -h "TestCase Non-critical" *.fsx | sed 's|^[[:space:]]*// TestCase Non-critical: |\t- |'
}
```


## Local test

replace `let arcDir =` line with 

```fsharp
// Local Test
let arcDir = fsi.CommandLineArgs.[1]
```

and run with

```bash
dotnet fsi ceplas-01-investigation/ceplas-01-investigation@1.0.0.fsx ~/datahub-dataplant/Facultative-CAM-in-Talinum/
dotnet fsi ceplas-02-biometadata/ceplas-02-biometadata@1.0.0.fsx ~/datahub-dataplant/Facultative-CAM-in-Talinum/
dotnet fsi ceplas-03-biodata/ceplas-03-biodata@1.0.0.fsx ~/datahub-dataplant/Facultative-CAM-in-Talinum/
```
