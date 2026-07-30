let [<Literal>]PACKAGE_METADATA = """(*
---
Name: ceplas-01-investigation
Summary: Validates the ARC's "biological" metadata.
Description: |

MajorVersion: 1
MinorVersion: 0
PatchVersion: 0
Publish: true
Authors:
  - FullName: Dominik Brilhaus
    Email: brilhaus@hhu.de
    Affiliation: CEPLAS
    AffiliationLink: https://ceplas.eu
  - FullName: Heinrich Lukas Weil
    Email: weil@nfdi4plants.org
    Affiliation: RPTU Kaiserslautern
    AffiliationLink: http://rptu.de/startseite
Tags:
  - Name: ceplas
  - Name: study
  - Name: assay
  - Name: quality-arc
ReleaseNotes: |
    Release leveled validation packages.
---
*)"""

#r "nuget: ARCExpect.Core, 7.0.0-alpha"
#r "nuget: ARCtrl.QueryModel, 3.0.0-alpha.4"
#r "nuget: Fable.SimpleHttp"

open ARCtrl
open ARCtrl.QueryModel
open Expecto
open ARCExpect
open System.IO



let hasAnnotationColumns (t: ARC)=
    t.ArcTables
    |> Seq.exists (fun t ->
        t.Columns
        |> Seq.exists (fun c ->
            c.Header.isCharacteristic || 
            c.Header.isParameter|| 
            c.Header.isFactor
        ))

let characteristicCount (t : ArcTable)=
    t.Columns
    |> Seq.filter (fun c -> c.Header.isCharacteristic)
    |> Seq.length

let parameterCount (t : ArcTable)=
    t.Columns
    |> Seq.filter (fun c -> c.Header.isParameter)
    |> Seq.length
    
let factorCount (t : ArcTable)=
    t.Columns
    |> Seq.filter (fun c -> c.Header.isFactor)
    |> Seq.length



// Input:

// let arcDir = Directory.GetCurrentDirectory()

//// TODO: remove ////////////////////
// Local Test
let arcDir = fsi.CommandLineArgs.[1]

////////////////////////

let arc =
    try ARC.load arcDir with
    | _ -> ARC(identifier = "placeholder")

arc.MakeDataFilesAbsolute()
arc.DataContextMapping()

// Validations

let criticalCases =     
    testList "criticalCases" [

    ////////////////////////////////////
    ////// ARC Study + Assay
    ////////////////////////////////////    
    
    // TestCase Critical: ARC contains at least one study or assay or workflow or run

    testCase "ARC contains at least one study or assay or workflow or run" <| fun _ ->

        if arc.StudyCount + arc.AssayCount + arc.WorkflowCount + arc.RunCount = 0 then
            failwith "ARC does not contain any study or assay or workflow or run"

    for s in arc.Studies do
        
        // TestCase Critical: Every study contains at least one annotation table
        testCase $"Study {s.Identifier} contains annotation table" <| fun _ ->
            if s.TableCount = 0 then
                failwith $"Study {s.Identifier} contains no annotation table"
        
        // TestCase Critical: Every study annotation table contains basic information
        // (more than 2 columns and 0 rows)
        
        for t in s.Tables do
            testCase $"Table {t.Name} of study {s.Identifier} contains basic information" <| fun _ ->
                
                if t.ColumnCount < 2 then
                    failwith $"Table {t.Name} contains less than 2 columns"
                if t.RowCount = 0 then
                    failwith $"Table {t.Name} contains no rows"

    for a in arc.Assays do
        
        // TestCase Critical: Every assay contains at least one annotation table
        testCase $"Assay {a.Identifier} contains annotation table" <| fun _ ->
            if a.TableCount = 0 then
                failwith $"Assay {a.Identifier} contains no annotation table"
        
        // TestCase Critical: Every assay annotation table contains basic information
        // (more than 2 columns and 0 rows)
        
        for t in a.Tables do
            testCase $"Table {t.Name} of assay {a.Identifier} contains basic information" <| fun _ ->
                
                if t.ColumnCount < 2 then
                    failwith $"Table {t.Name} contains less than 2 columns"
                if t.RowCount = 0 then
                    failwith $"Table {t.Name} contains no rows"
                    
        for r in arc.Runs do
        
        // TestCase Critical: Every run contains at least one annotation table
        testCase $"Run {r.Identifier} contains annotation table" <| fun _ ->
            if r.TableCount = 0 then
                failwith $"Run {r.Identifier} contains no annotation table"
        
        // TestCase Critical: Every run annotation table contains basic information
        // (more than 2 columns and 0 rows)
        
        for t in r.Tables do
            testCase $"Table {t.Name} of run {r.Identifier} contains basic information" <| fun _ ->
                
                if t.ColumnCount < 2 then
                    failwith $"Table {t.Name} contains less than 2 columns"
                if t.RowCount = 0 then
                    failwith $"Table {t.Name} contains no rows"




    ]
    

let nonCriticalCases =
    testList "nonCriticalCases" [
    



    ]

// Execution:
Setup.ValidationPackage(
    metadata = Setup.Metadata(
        PACKAGE_METADATA,
        AVPRIndex.Frontmatter.FrontmatterLanguage.FSharpFrontmatter
        ),
    CriticalValidationCases = [criticalCases],
    NonCriticalValidationCases = [nonCriticalCases]
)
|> Execute.ValidationPipeline(
    basePath = arcDir
)