#r "nuget: ARCExpect.Core, 7.0.0-alpha"
#r "nuget: ARCtrl.QueryModel, 3.0.0-alpha.4"

open ARCtrl
open ARCtrl.QueryModel
open System.IO

let home = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile)
let arcDir = home + "/datahub-dataplant/Facultative-CAM-in-Talinum/"

let arc = ARC.load arcDir


for  d in arc.ArcTables.Data do
    printfn $"####{d.Name}"
    printfn $"#### {d.FirstSamples.IsEmpty}"

    let fsBlank = 
        d.FirstSamples
        |> List.exists (fun q -> q.Name = "")

    printfn "%b" fsBlank

    printfn $"{d.Sources.Count}"

arc.ArcTables.Data
|> Seq.map (fun d -> d.FirstSamples |> List.exists (fun q -> q.Name = ""))
