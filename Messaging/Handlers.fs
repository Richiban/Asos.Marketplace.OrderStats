namespace Asos.Marketplace.OrderStats.Messaging

open System
open System.Threading.Tasks
open NServiceBus

open Asos.Marketplace.OrderStats.Messages

type OrderPlacedHandler() =
    interface IHandleMessages<OrderPlaced> with
        member this.Handle (msg, ctx) =
            Console.WriteLine("We're in the handler!")

            Task.CompletedTask