namespace Asos.Marketplace.OrderStats

open NServiceBus
open NServiceBus.Features
open Asos.Marketplace.OrderStats.Service.EventHandlers

type EndpointConfig() =
    let configureIoc (endpointConfiguration : EndpointConfiguration) =
        endpointConfiguration.RegisterComponents(
            registration = 
                fun configureComponents ->
                    let factory() = OrderPlacedEventHandler (createOrderPlacedEventHandler ())

                    configureComponents.ConfigureComponent<OrderPlacedEventHandler>(factory, dependencyLifecycle = DependencyLifecycle.SingleInstance)
                )

        ()

    interface IConfigureThisEndpoint with
        member this.Customize endpointConfiguration =
            endpointConfiguration.DefineEndpointName("Asos.Marketplace.OrderStats")
            endpointConfiguration.SendFailedMessagesTo("Asos.Marketplace.OrderStats.Error")
            endpointConfiguration.AuditProcessedMessagesTo("Asos.Marketplace.OrderStats.Audit")

            let transport = endpointConfiguration.UseTransport<MsmqTransport>()
            let persistence = endpointConfiguration.UsePersistence<InMemoryPersistence>()

            let conventions = endpointConfiguration.Conventions()
            
            conventions.DefiningEventsAs(fun t -> t.Namespace.EndsWith("Events")) |> ignore

            endpointConfiguration.DisableFeature<TimeoutManager>()

            configureIoc endpointConfiguration

            ()