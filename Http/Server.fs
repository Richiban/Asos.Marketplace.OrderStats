module Asos.Marketplace.OrderStats.Http.Server

open System
open System.Threading
open Suave
open System.Net
open Asos.Marketplace.OrderStats.Service.Factories
open Asos.Marketplace.OrderStats.Domain

let view model =
    sprintf <|
    "<html>
        <head>
            <style type=\"text/css\">
            body {
                color: white;
                font-family: sans-serif;
                background-color: 333;
                text-align: center;
            }

            h1 {
                width: 100%%;
            }

            .container {
                display: flex;
                justify-content: center;
                margin: auto;
            }

            .item {                
                padding: 20px;
                height: 100px; /* Or whatever */
                margin: 8px;  
                background: blue;
                display: flex;
                flex-direction: column;
                border: 1px solid silver;
                border-top: 8px solid #f3eeee;
            }

            .key, .value {
                flex-grow: 1;
            }

            .key {
                text-shadow: 3px 0 6px #111, -3px 0 6px #111;
            }

            .value {
                font-size: 3em;
                background: rgba(0, 0, 0, 0.2);
            }

            .item:nth-child(1) {             
                background: lightgreen;
                border-top-left-radius: 6px;
                border-bottom-left-radius: 6px;
            }

            .item:nth-child(2) {             
                background: antiquewhite;
            }

            .item:nth-child(3) {             
                background: pink;
            }

            .item:nth-child(4) {             
                background: lightblue;
            }

            .item:nth-child(5) {             
                background: orange;
                border-top-right-radius: 6px;
                border-bottom-right-radius: 6px;
            }
            </style>
        </head>
        <body>
            <h1>Showing statistics for %s</h1>
            <div class=\"container\">
                <div class=\"item\">
                    <div class=\"key\">
                        Average Order Size
                    </div>
                    <div class=\"value\">
                        %A
                    </div>
                </div>
                <div class=\"item\">
                    <div class=\"key\">
                        Average Order Value
                    </div>
                    <div class=\"value\">
                        &pound;%M
                    </div>
                </div>
                <div class=\"item\">
                    <div class=\"key\">
                        Total Item Count
                    </div>
                    <div class=\"value\">
                        %A
                    </div>
                </div>
                <div class=\"item\">
                    <div class=\"key\">
                        Total Order Count
                    </div>
                    <div class=\"value\">
                        %A
                    </div>
                </div>
                <div class=\"item\">
                    <div class=\"key\">
                        Total Order Value
                    </div>
                    <div class=\"value\">
                        &pound;%M
                    </div>
                </div>
            <div>
        </body>
    </html>" <| (model.Date.ToShortDateString())
             <| model.AverageOrderSize
             <| model.AverageOrderValue
             <| model.TotalItemCount
             <| model.TotalOrderCount
             <| model.TotalOrderValue

let app () = 
    let queryHandler = createGetDailyStatsQueryHandler ()

    let stats =
        queryHandler { SellerId = SellerId (Guid.Parse("577316f9-31ea-493f-b630-3fb153b0a891")); Date = DateTime.Today }
        
    Successful.OK (view stats)
    

[<EntryPoint>]
let main argv = 
    let cts = new CancellationTokenSource()
    let port = UInt16.Parse("8083")
    let bindings = [{ scheme = Protocol.HTTP; socketBinding = { ip = IPAddress.Parse("127.0.0.1"); port = port }}]
    let conf = { defaultConfig with cancellationToken = cts.Token; bindings = bindings }
    let listening, server = startWebServerAsync conf (app ())
    
    Async.Start(server, cts.Token)
    printfn "Make requests now"
    Console.ReadKey true |> ignore
    
    cts.Cancel()

    0 // return an integer exit code