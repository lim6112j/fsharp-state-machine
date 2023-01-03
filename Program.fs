// For more information see https://aka.ms/fsharp-console-apps
namespace CustomStateMachine
open StateMachine
open DoorMachine
open CustomStateMachine.Better.Process.StateMachine
module Program =
    DoorMachine.test (printfn "%s")
    //System.Console.ReadLine() |> ignore
    let init() = stateMachineState Ready
    let result = init()
    let result1 = result.AllowedEvents.[0].RaiseEvent()
    printfn "%A" result1
