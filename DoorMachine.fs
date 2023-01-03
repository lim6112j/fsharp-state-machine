namespace CustomStateMachine
open StateMachine
module DoorMachine =
  open StateMachine
  type Event = | Open | Close | Lock | Unlock
  let configureDoor sound =
    let rec opened event =
      match event with
      | Close -> sound "bang"; Next (closed false)
      | Lock -> sound "clack"; Next opened
      | _ -> Next opened
    and closed locked event =
      match event with
      | Open when locked -> sound "dunmdum"; Next (closed locked)
      | Open -> sound "squeak"; Next opened
      | Lock -> sound "click"; Next (closed true)
      | Unlock -> sound "clack"; Next (closed false)
      | _ -> Next (closed locked)
    Next (closed false)
  let test sound = 
    let agent = sound |> configureDoor |> StateMachine.createAgent
    agent.Post Lock
    agent.Post Unlock
    agent.Post Open
    agent.Post Close
