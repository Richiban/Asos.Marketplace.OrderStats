namespace Asos.Marketplace.OrderStats

open NServiceBus
open NServiceBus.Features
open Asos.Marketplace.OrderStats.Messages

type EndpointConfig() =
    interface IConfigureThisEndpoint with
        member this.Customize endpointConfiguration =
            endpointConfiguration.DefineEndpointName("Asos.Marketplace.OrderStats")
            endpointConfiguration.SendFailedMessagesTo("Asos.Marketplace.OrderStats.Error")
            endpointConfiguration.AuditProcessedMessagesTo("Asos.Marketplace.OrderStats.Audit")

            let transport = endpointConfiguration.UseTransport<MsmqTransport>()
            let persistence = endpointConfiguration.UsePersistence<InMemoryPersistence>()

            endpointConfiguration.Conventions().DefiningMessagesAs(fun t -> t.Assembly = typeof<OrderPlaced>.Assembly)

            endpointConfiguration.DisableFeature<TimeoutManager>()

            ()