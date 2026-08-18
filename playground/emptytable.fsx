#r "nuget: ARCExpect.Core, 7.0.0-alpha"
#r "nuget: ARCtrl.QueryModel, 3.0.0-alpha.4"

open ARCtrl
open ARCtrl.QueryModel
open System.IO
open Fable.SimpleHttp

let arcDir = System.IO.Path.Combine(__SOURCE_DIRECTORY__, "../tests/fixtures/02-Fails-03")

let arc = ARC.load arcDir

let isEmptyAnnoTable (iov : IOValueCollection) =
    iov
    |> Seq.forall (fun kv -> not kv.Value.HasValue)

let emptyAnnoCols (iov : IOValueCollection) =
    iov
    |> Seq.groupBy (fun kv -> kv.Value.NameText)
    |> Seq.choose (fun (header, values) ->
        if values |> Seq.forall (fun kv -> not kv.Value.HasValue) then
            Some header
        else
            None
    )


for a in arc.Assays do
    for t in a.Tables do

    let allEmpty = isEmptyAnnoTable t.ISAValues

    printfn $"All empty: {allEmpty}"

    let emptyCols = emptyAnnoCols t.ISAValues

    for key1 in emptyCols do
        printfn $"Empty column: {key1}"
