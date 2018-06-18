namespace Asos.Marketplace.OrderStats.Events

open System

[<CLIMutable>]
type OrderPlaced =
    { OrderId : Guid
      SellerId : Guid
      PlacedOn : DateTime
      TotalValue : decimal
      ItemCount : int }