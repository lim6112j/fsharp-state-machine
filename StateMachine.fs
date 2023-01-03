namespace CustomStateMachine
module StateMachine =
  type State<'Event> =
    | Next of ('Event -> State<'Event>)
    | Stop
  let feed state event =
    match state with
    | Stop -> failwith("Terminal state reached")
    | Next handler -> event |> handler
  type StateMachine<'event>(initial: State<'event>) =
    let mutable current = initial
    member this.Fire event = current <- feed current event
    member this.isStopped
      with get () = match current with | Stop -> true | _ -> false
  let createMachine initial = StateMachine(initial)
  let createAgent initial =
    MailboxProcessor.Start (fun inbox ->
      let rec loop state = async {
        let! event = inbox.Receive()
        match event |> feed state with
        | Stop -> ()
        | Next _ as next -> return! loop next
      }
      loop initial
  )
