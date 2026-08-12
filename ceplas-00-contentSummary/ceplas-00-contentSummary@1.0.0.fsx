let [<Literal>]PACKAGE_METADATA = """(*
---
Name: ceplas-00-contentSummary
Summary: This does not "validate" ARC content, but simply report some identified metadata.
Description: |
    ## Report contents of the ARC
MajorVersion: 1
MinorVersion: 0
PatchVersion: 0
Publish: true
Authors:
  - FullName: Dominik Brilhaus
    Email: brilhaus@hhu.de
    Affiliation: CEPLAS
    AffiliationLink: https://ceplas.eu
Tags:
  - Name: ceplas
  - Name: report
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


let formatOptional (os : option<'T>) =
    match os with
    | Some value ->
        match box value with
        | :? string as s -> s
        | _ -> sprintf "%A" value
    | None -> ""


// Validations

let reportCases =     
    testList "reportCases" [

    ////////////////////////////////////
    ////// Investigation
    ////////////////////////////////////

    
    testCase 
        (sprintf 
            "Report: Investigation Title:" +
            formatOptional arc.Title  
        )
        <| fun _ -> ()
    
    testCase 
        (sprintf 
            "Report: Investigation Description:" +
            formatOptional arc.Description  
        )
        <| fun _ -> ()

    for c in arc.Contacts do
    
        testCase 
            (sprintf 
                "Report: Investigation Contact: %s %s"
                (formatOptional c.FirstName)
                (formatOptional c.LastName)
            )
            <| fun _ -> ()

    testCase
        (sprintf
            "Report: ARC content: studies=%d, assays=%d, workflows=%d, runs=%d"
            arc.StudyCount
            arc.AssayCount
            arc.WorkflowCount
            arc.RunCount
            )
        <| fun _ -> ()

    ////////////////////////////////////
    ////// Studies
    ////////////////////////////////////

    for s in arc.Studies do

        testCase (sprintf $"Report: Study {s.Identifier} title: {formatOptional s.Title}" ) <| fun _ -> ()
        testCase (sprintf $"Report: Study {s.Identifier} contains {s.TableCount} table(s)" ) <| fun _ -> ()

        for t in s.Tables do
        
            testCase (sprintf $"Report: Study Table {s.Identifier}-{t.Name} contains {t.ColumnCount} column(s)" ) <| fun _ -> ()
            testCase (sprintf $"Report: Study Table {s.Identifier}-{t.Name} contains {t.RowCount} row(s)" ) <| fun _ -> ()
    

    ////////////////////////////////////
    ////// Assays
    ////////////////////////////////////

    for a in arc.Assays do

        testCase (sprintf $"Report: Assay {a.Identifier} title: {formatOptional a.Title}" ) <| fun _ -> ()
        testCase (sprintf $"Report: Assay {a.Identifier} contains {a.TableCount} table(s)" ) <| fun _ -> ()

        for t in a.Tables do
        
            testCase (sprintf $"Report: Assay Table {a.Identifier}-{t.Name} contains {t.ColumnCount} column(s)" ) <| fun _ -> ()
            testCase (sprintf $"Report: Assay Table {a.Identifier}-{t.Name} contains {t.RowCount} row(s)" ) <| fun _ -> ()
    
    ////////////////////////////////////
    ////// Samples and Data nodes
    ////////////////////////////////////

    for t in arc.ArcTables do

        testCase (sprintf $"Report: Table {t.Name} input type: {t.InputType}" ) <| fun _ -> ()
        testCase (sprintf $"Report: Table {t.Name} input count: {t.InputNames.Length}" ) <| fun _ -> ()
        testCase (sprintf $"Report: Table {t.Name} output count: {t.OutputNames.Length}" ) <| fun _ -> ()
    
                
    testCase (sprintf $"Report: Data count: {arc.ArcTables.Data.Count}" ) <| fun _ -> ()   


    ]    


// Execution:
Setup.ValidationPackage(
    metadata = Setup.Metadata(
        PACKAGE_METADATA,
        AVPRIndex.Frontmatter.FrontmatterLanguage.FSharpFrontmatter
        ),
    NonCriticalValidationCases = [reportCases]
)
|> Execute.ValidationPipeline(
    basePath = arcDir
)