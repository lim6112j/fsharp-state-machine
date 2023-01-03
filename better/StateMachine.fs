namespace CustomStateMachine.Better
module Process =
    module StateMachine =
        type ProcessState =
            | New
            | Ready
            | Running
            | Waiting
            | Terminated
        
        type ProcessEvent =
            | Admit
            | Interrupt
            | SchedulerDispatch
            | IOEventWait
            | IOEventCompletion
            | Exit
        type AllowedEvent =
            {
            EventInfo : ProcessEvent
            RaiseEvent : unit -> EventResult
            }
        and EventResult =
            {
            CurrentState : ProcessState
            AllowedEvents : AllowedEvent array
            }

        let private stateTransition event =
         match event with
            | Admit -> Ready
            | SchedulerDispatch -> Running
            | IOEventWait -> Waiting
            | Exit -> Terminated
            | Interrupt -> Ready
            | IOEventCompletion -> Ready
        
        let private getEventForState state =
         match state with
            | New ->[|Admit|]
            | Ready -> [|SchedulerDispatch|]
            | Running -> [|IOEventWait; Interrupt; Exit|]
            | Waiting -> [|IOEventCompletion|]
            | Terminated -> [||]

        let rec stateMachine event =
         let newState = stateTransition event
         let newEvents = getEventForState newState
         {
            CurrentState = newState
            AllowedEvents =
            newEvents
            |> Array.map (fun e ->
                          let f() = stateMachine e
                          {
                EventInfo = e
                RaiseEvent = f
            })
         }
        let rec stateMachineState state =
         let newEvents = getEventForState state
         {
            CurrentState = state
            AllowedEvents =
            newEvents
            |> Array.map (fun e ->
                          let f() = stateMachine e
                          {
                EventInfo = e
                RaiseEvent = f
            })
         }
