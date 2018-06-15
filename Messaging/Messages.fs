namespace Asos.Marketplace.OrderStats.Messages

open System

[<CLIMutable>]
type OrderPlaced =
    { OrderId : Guid
      TotalValue : decimal
      ItemCount : int }