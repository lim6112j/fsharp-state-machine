// For more information see https://aka.ms/fsharp-console-apps
namespace CustomStateMachine
open StateMachine
open DoorMachine
module Program =
    DoorMachine.test (printfn "%s")
    System.Console.ReadLine() |> ignore
