namespace Asos.Marketplace.OrderStats.Data

open Asos.Marketplace.OrderStats.Domain
open System
open System.IO
open Newtonsoft.Json

type DailyStatsRepository (storageDirectory : string) =
    let getFileName (day : DateTime) =
        let fileName = sprintf "DataStore_%s.txt" (day.ToString("yyyy_MM_dd"))
        Path.Combine [|storageDirectory; fileName|]

    let getFileLinesOrEmpty fileName =
        if not (File.Exists fileName)
        then Seq.empty
        else File.ReadLines fileName

    let loadModelFromStore (dayToLoad : DateTime) =
        getFileName dayToLoad
        |> getFileLinesOrEmpty
        |> Seq.map JsonConvert.DeserializeObject<DailyStatistics>

    let writeModelToStore (dayToLoad : DateTime) (stats : DailyStatistics seq) =
        stats
        |> Seq.map (fun x -> JsonConvert.SerializeObject(x, Formatting.None))
        |> Array.ofSeq
        |> fun x -> File.WriteAllLines (getFileName dayToLoad, x)

    member this.Retrieve : RetrieveDailyStatistics = 
        fun dateTime sellerId ->
            loadModelFromStore (dateTime.Date)
            |> Seq.tryFind (fun row -> row.SellerId = sellerId)
            |> Option.defaultValue (DailyStatistics.Zero sellerId dateTime)

    member this.Save : SaveDailyStatistics = 
        fun dateTime stats ->
            loadModelFromStore (stats.Date)
            |> Seq.filter (fun row -> row.SellerId <> stats.SellerId)
            |> Seq.append [stats]
            |> writeModelToStore stats.Date