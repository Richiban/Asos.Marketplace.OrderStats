namespace Asos.Marketplace.OrderStats.Domain

open System

type SellerId = SellerId of Guid
type OrderId = OrderId of Guid
type Money = decimal

type OrderStats = 
    { OrderId : OrderId
      SellerId : SellerId
      PlacedOn : DateTime
      TotalValue : Money
      ItemCount : int }

type DailyStatistics =
    { SellerId : SellerId
      Date : DateTime
      AverageOrderValue : Money
      AverageOrderSize : float 
      TotalOrderCount : int
      TotalItemCount : int
      TotalOrderValue : Money }

    static member (+) (dailyStats : DailyStatistics, orderStats : OrderStats) =
        let totalOrderCount = dailyStats.TotalOrderCount + 1
        let totalItemCount = dailyStats.TotalItemCount + orderStats.ItemCount
        let totalOrderValue = dailyStats.TotalOrderValue + orderStats.TotalValue
                    
        { dailyStats with
            TotalOrderCount = totalOrderCount
            TotalItemCount = totalItemCount
            AverageOrderSize = (float totalItemCount) / (float totalOrderCount)
            AverageOrderValue = totalOrderValue / (decimal totalOrderCount)
            TotalOrderValue = totalOrderValue }

    static member Zero sellerId date = 
        { SellerId = sellerId
          Date = date
          TotalOrderCount = 0
          TotalItemCount = 0
          AverageOrderSize = 1.0
          AverageOrderValue = 1m
          TotalOrderValue = 0m }
    

type RetrieveDailyStatistics = DateTime -> SellerId -> DailyStatistics

type SaveDailyStatistics = DateTime -> DailyStatistics -> unit