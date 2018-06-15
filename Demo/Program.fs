open NServiceBus

open Asos.Marketplace.OrderStats.Messages
open System
open NServiceBus.Features

[<EntryPoint>]
let main argv = 

    let endpointConfiguration = EndpointConfiguration("Asos.Marketplace.Orders.Messaging")

    endpointConfiguration.UseTransport<MsmqTransport>()
    endpointConfiguration.DisableFeature<MessageDrivenSubscriptions>()

    endpointConfiguration.SendOnly()

    endpointConfiguration.Conventions().DefiningMessagesAs(fun t -> t = typeof<OrderPlaced>)

    let endpointInstance = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult()

    endpointInstance.Send("Asos.Marketplace.OrderStats", { OrderId = Guid.NewGuid(); ItemCount = 1; TotalValue = 50.0m }).Wait()

    0 // return an integer exit code
