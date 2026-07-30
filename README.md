
# CEPLAS validation packages

Dev area for CEPLAS validation packages

- Submit here: 
  - https://github.com/nfdi4plants/arc-validate-package-registry
  - https://avpr.nfdi4plants.org


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
let home = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile)
let arcDir = home + "/datahub-dataplant/Facultative-CAM-in-Talinum/"
```