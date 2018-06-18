module Asos.Marketplace.OrderStats.Service.Factories

open Asos.Marketplace.OrderStats.Data
open Asos.Marketplace.OrderStats.Service.EventHandlers

let createDailyStatsRepository () =
    let binPath = @"C:\Source\Richiban\Asos.Marketplace.OrderStats"//System.IO.Directory.GetCurrentDirectory()

    DailyStatsRepository binPath

let createOrderPlacedEventHandler () =
    let repo = createDailyStatsRepository ()

    orderPlacedEventHandler repo.Retrieve repo.Save

let createGetDailyStatsQueryHandler () =
    let repo = createDailyStatsRepository ()

    getDailyStatsQueryHandler repo.Retrieve