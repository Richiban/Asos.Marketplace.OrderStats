module Asos.Marketplace.OrderStats.Service.EventHandlers

open Asos.Marketplace.OrderStats.Events
open Asos.Marketplace.OrderStats.Domain
open System
open Asos.Marketplace.OrderStats.Data
open System.IO

type OrderPlacedEventHandler = OrderPlacedEventHandler of (OrderPlaced -> unit)

let orderPlacedEventHandler (getStats : RetrieveDailyStatistics) (saveStats : SaveDailyStatistics) (orderPlacedEvent : OrderPlaced) =
    let mapToDomain (x : OrderPlaced) : OrderStats = 
        { OrderId = OrderId x.OrderId
          SellerId = SellerId x.SellerId
          PlacedOn = x.PlacedOn
          TotalValue = x.TotalValue
          ItemCount = x.ItemCount }

    let statsDate = DateTime.Today
    let sellerId = SellerId orderPlacedEvent.SellerId
    let stats = getStats statsDate sellerId
    let todaysStats = mapToDomain orderPlacedEvent
    let newStats = stats + todaysStats
    
    saveStats statsDate newStats

let createOrderPlacedEventHandler () =
    let repo = 
        let binPath = System.IO.Directory.GetCurrentDirectory()

        DailyStatsRepository binPath

    orderPlacedEventHandler repo.Retrieve repo.Save