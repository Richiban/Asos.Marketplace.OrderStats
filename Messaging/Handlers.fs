namespace Asos.Marketplace.OrderStats.Messaging

open System
open System.Threading.Tasks
open NServiceBus

open Asos.Marketplace.OrderStats.Events
open Asos.Marketplace.OrderStats.Service.EventHandlers

type OrderPlacedHandler (serviceHandler : OrderPlacedEventHandler) =
    interface IHandleMessages<OrderPlaced> with
        member this.Handle (msg, ctx) =
            Console.WriteLine("We're in the handler!")
            
            let (OrderPlacedEventHandler serviceHandler) = serviceHandler
            serviceHandler msg

            Task.CompletedTask