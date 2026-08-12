#r "nuget: ARCExpect.Core, 7.0.0-alpha"
#r "nuget: ARCtrl.QueryModel, 3.0.0-alpha.4"

open ARCtrl
open ARCtrl.QueryModel
open System.IO
open Fable.SimpleHttp


let home = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile)
let arcDir = home + "/datahub-dataplant/Facultative-CAM-in-Talinum/"

let arc = ARC.load arcDir


let pathIsUrl (p: string) =
    p.StartsWith("http:") || p.StartsWith("https:")

type URLStatus =
    | Malformed
    | Resolves
    | Fails

let urlResolves (url: string) =

    async {
        try
            let! (statusCode, responseText) = Http.get url

            match statusCode with
            | 200 -> return Resolves
            | _ -> return Fails

        with
            | _ -> return Malformed
    }
    |> Async.RunSynchronously


type ArcTable with
    member this.TryGetProtocolUriColumn() =
        this.TryGetColumnByHeader(CompositeHeader.ProtocolUri)



// for a in arc.Assays do

//     printfn $"{a.Identifier}"

//     for t in a.Tables do
        
//         printfn $"{t.Name}"
        
//         if t.TryGetProtocolUriColumn().IsSome then
        
//             let pu = t.GetProtocolUriColumn()

//             let protocolPaths = pu.Cells |> Seq.distinctBy (fun d -> d.AsFreeText)

//             protocolPaths
//             |> Seq.iter (fun p -> 

//                 let filePath = p.AsFreeText
//                 printfn "%s" filePath
                
//                 if pathIsUrl filePath then
//                         match urlResolves filePath with
//                         | Resolves -> ()
//                         | Fails -> failwith $"Url {filePath} in assay {a.Identifier} could not be resolved"
//                         | Malformed -> failwith $"Url {filePath} in assay {a.Identifier} is malformed"                
//                 )

//         else
//             printfn $"      No Protocol Uri"


for t in arc.ArcTables do    
    printfn $"{t.Name}"

    printfn $"{t.RowCount}"

    // let row1 = t.GetRow 1
    // printfn $"{t.Rows.IsEmpty}"
        
        

