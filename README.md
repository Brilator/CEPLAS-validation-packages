
# CEPLAS validation packages

- Submit here: 
  - https://github.com/nfdi4plants/arc-validate-package-registry
  - https://avpr.nfdi4plants.org

## Design

- Incremental, but independent?
- While (chron)logically building on one another (and at least in part mutually exclusive), every package should be useful for itself 
- So even if no investigation metadata exists, one can still check for high-level (meta)data annotation

### ceplas-01-investigation/
  - checks only root ARC content and investigation
### ceplas-02-biometadata/
  - checks for existence and ISA-representation of metadata (no dataset files)
  - should at this abstraction be applicable to any lab-experimental, computational or purely consuming modeling ARC
### ceplas-03-biodata/
  - checks for existence ISA-integration of data
### ceplas-04-connectedData/
  - checks that metadata items are connected along the ISA-graph / ISA processes

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
