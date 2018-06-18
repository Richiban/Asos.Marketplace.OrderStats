open NServiceBus

open Asos.Marketplace.OrderStats.Events
open System
open NServiceBus.Features

[<EntryPoint>]
let main argv = 

    let endpointConfiguration = EndpointConfiguration("Asos.Marketplace.Orders.Messaging")

    endpointConfiguration.UseTransport<MsmqTransport>() |> ignore
    endpointConfiguration.DisableFeature<MessageDrivenSubscriptions>()

    endpointConfiguration.SendOnly()

    let conventions = endpointConfiguration.Conventions()
    
    conventions.DefiningMessagesAs(fun t -> match t.Namespace with null -> false | ns -> ns.EndsWith("Events")) |> ignore

    let endpointInstance = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult()
    
    let message = { SellerId = Guid("577316f9-31ea-493f-b630-3fb153b0a891"); OrderId = Guid.NewGuid(); ItemCount = 1; TotalValue = 50.0m; PlacedOn = DateTime.Today }

    endpointInstance.Send("Asos.Marketplace.OrderStats", message).Wait()

    0 // return an integer exit code