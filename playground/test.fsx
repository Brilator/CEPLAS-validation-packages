#r "nuget: ARCExpect.Core, 7.0.0-alpha"
#r "nuget: ARCtrl.QueryModel, 3.0.0-alpha.4"

open ARCtrl
open ARCtrl.QueryModel
open System.IO

let home = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile)
let arcDir = home + "/datahub-dataplant/Facultative-CAM-in-Talinum/"

let arc = ARC.load arcDir

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

if hasAnnotationColumns arc then

  for t in arc.ArcTables do
    let annoColCount = characteristicCount t + parameterCount t + factorCount t
    if annoColCount = 0 then
      printfn $"Annotation table {t.Name} contains no annotation column"


