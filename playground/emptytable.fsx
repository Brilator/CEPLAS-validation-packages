#r "nuget: ARCExpect.Core, 7.0.0-alpha"
#r "nuget: ARCtrl.QueryModel, 3.0.0-alpha.4"

open ARCtrl
open ARCtrl.QueryModel
open System.IO
open Fable.SimpleHttp

let arcDir = System.IO.Path.Combine(__SOURCE_DIRECTORY__, "../tests/fixtures/04-Fails-02-emptyTable")

let arc = ARC.load arcDir

let allEmptyISAValues (iov : IOValueCollection) =
    iov
    |> Seq.forall (fun kv -> not kv.Value.HasValue)

let emptyKeys (iov : IOValueCollection) =
    iov
    |> Seq.filter (fun kv -> not kv.Value.HasValue)
    |> Seq.map (fun kv -> kv.Key)

for t in arc.ArcTables do

    let allEmpty = allEmptyISAValues t.ISAValues

    printfn $"All empty: {allEmpty}"

    let emptyCols = emptyKeys t.ISAValues

    printfn $"All empty: {emptyCols}"