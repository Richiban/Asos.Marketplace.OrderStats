module Asos.Marketplace.OrderStats.Service.EventHandlers

open Asos.Marketplace.OrderStats.Events
open Asos.Marketplace.OrderStats.Domain
open System

type OrderPlacedEventHandler = OrderPlacedEventHandler of (OrderPlaced -> unit)

type GetDailyStatsQuery = { SellerId : SellerId; Date : DateTime }

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
    let newStats = stats.add todaysStats
    
    saveStats statsDate newStats

let getDailyStatsQueryHandler (getStats : RetrieveDailyStatistics) (query : GetDailyStatsQuery) =
    getStats query.Date query.SellerId