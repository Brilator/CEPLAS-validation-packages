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


let t1 = arc.ArcTables[1]

t1.ISAValues
|> Seq.map (fun kv -> kv.Value.NameText, kv.Value.HasValue)
|> Seq.toList


t1.ColumnCount


for t in arc.ArcTables do

    printfn $"{t.Name}"
    printfn $"{t.ColumnCount}"

    let emptyRows =
        t.ISAValues
        |> Seq.chunkBySize t.ColumnCount
        |> Seq.mapi (fun rowIndex row -> rowIndex, row)
        |> Seq.choose (fun (rowIndex, row) ->
            if row |> Seq.forall (fun kv -> not kv.Value.HasValue) then
                Some rowIndex
            else
                None
        )

    for rowIndex in emptyRows do
        printfn $"Empty row: {rowIndex}"

t1.Name
t1.ColumnCount

t1.ISAValues
|> Seq.chunkBySize t1.ColumnCount
|> Seq.last
|> Seq.iter (fun kv ->
    printfn "%s -> HasValue=%b"
        kv.Value.NameText
        kv.Value.HasValue)

let test =
    t1.ISAValues
    |> Seq.chunkBySize t1.ColumnCount
    |> Seq.last
    |> Seq.forall (fun kv -> not kv.Value.HasValue)

printfn $"Last row empty: {test}"


let emptyRows (t: ArcTable) =
    let rowSize =
        t.ISAValues
        |> Seq.map (fun kv -> kv.Value.NameText)
        |> Seq.distinct
        |> Seq.length

    t.ISAValues
    |> Seq.chunkBySize rowSize
    |> Seq.mapi (fun rowIndex row ->
        rowIndex,
        row |> Seq.forall (fun kv -> not kv.Value.HasValue)
    )
    |> Seq.choose (fun (rowIndex, isEmpty) ->
        if isEmpty then Some rowIndex
        else None
    )

let emptyRows (t: ArcTable) =
    [0 .. t.RowCount - 1]
    |> Seq.choose (fun rowIndex ->
        let row = t.GetRow(rowIndex)

        if row |> Seq.forall (fun kv -> not kv.Value.HasValue) then
            Some rowIndex
        else
            None
    )


let emptyRrs = emptyRows t1

for rowIndex in emptyRrs do
    printfn $"Empty row: {rowIndex}"


let r1 = t1.GetRow (1, false)

t1.RowCount

typeof<ArcTable>.GetMethods()
|> Seq.filter (fun m -> m.Name = "GetRow")
|> Seq.iter (fun m -> printfn "%A" m)

typeof<CompositeCell>.GetProperties()
|> Seq.iter (fun p -> printfn "%s : %s" p.Name p.PropertyType.FullName)

t1.GetRow(0)

t1.GetRow(6)
|> Seq.iteri (fun i cell ->
    printfn "%d: %A" i cell)

let rs = [0 .. t1.RowCount - 1]



let isEmptyCell (cell: CompositeCell) =
    cell.isFreeText && cell.AsFreeText = ""
    ||
    cell.isTerm && cell.AsTerm.NameText = ""
    ||
    cell.isUnitized && fst cell.AsUnitized = ""
    ||
    cell.isData && cell.AsData.NameText = ""

let emptyRows (t: ArcTable) =
    [0 .. t.RowCount - 1]
    |> Seq.choose (fun rowIndex ->
        let row = t.GetRow rowIndex

        if row |> Seq.forall isEmptyCell then
            Some rowIndex
        else
            None
    )

emptyRows t1
|> Seq.iter (printfn "Empty row: %d")
