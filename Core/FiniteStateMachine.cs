using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Core
{
    public partial class FiniteStateMachine : Node
    {


        /**
         * Constructor for the FiniteStateMachine.
         * @param initialState The initial state of the finite state machine.
         */
        public FiniteStateMachine(
            List<Godot.Node> nodes
        )
        {
            this._states = new Dictionary<string, FiniteState>();
            this.RegisterStatesFromNodes(nodes);
        }

        /**
          * Current state of the finite state machine.
          */
        public FiniteState _currentState;
        public FiniteState CurrentState => _currentState;


        /**
          * Dictionary to map states to their corresponding actions.
          */
        private readonly Dictionary<string, FiniteState> _states;

        public List<string> AvailableStates => _states.Keys.ToList();

        public void RegisterStatesFromNodes(
            List<Godot.Node> nodes
        )
        {


            foreach (Node node in nodes)
            {

                if (!node.HasMeta("State"))
                {
                    GD.Print("Node does not have a State meta data. Skipping: " + node);
                    continue;
                }

                this.RegisterState((FiniteState)node);
            }
        }


        /**
          * Registers a new state with its corresponding action.
          */

        public void RegisterState(FiniteState state)
        {
            GD.Print($"Registering State: {state.name} > {state}");
            this._states[state.name] = state;
            state.OnRegister(this);
        }

        /**
        * Changes the state of the finite state machine.
         * @param newState The new state to change to.
         * @throws ArgumentException if the new state is not registered.
         */
        public void Transition(string stateName)
        {
            var name = stateName.ToLower();
            if (!this._states.ContainsKey(name))
            {
                throw new ArgumentException($"State {stateName} is not registered.");
            }

            FiniteState _state = this._states[name];
            GD.Print($"TransitionTo: {_state}, Name: {_state.name}");
            FiniteState _previousState = this._currentState;
            if (_previousState != null)
            {
                GD.Print($"TransitionFrom: {_previousState}, Name: {_previousState.name}");
            }

            this._currentState = null;
            if (_previousState != null)
            {

                _previousState.Exit();
            }

            this._currentState = _state;
            GD.Print($"TransitionDone: ${this._currentState}, Name: {_currentState.name}");
            this._currentState.Enter();
        }


        public bool HasState(string stateName)
        {
            return this._states.ContainsKey(stateName.ToLower());
        }

        public void Update(double delta)
        {
            if (this.CurrentState == null)
            {
                return;
            }

            this.CurrentState.Update(delta);
        }

    }

    public partial class FiniteState : Node
    {
        public FiniteStateMachine machine
        {
            get; set;
        }

        public override void _Ready()
        {
            this.SetMeta("State", this);
            GD.Print($"Ready: {this}");
        }

        public string name
        {
            get
            {
                return this.Name.ToString().ToLower();
            }
        }

        public virtual void OnRegister(FiniteStateMachine machine)
        {
            this.machine = machine;
            GD.Print($"Registered State: {this.name} > {this}");

        }

        public virtual void Enter() { }

        public virtual void Update(double delta) { }

        public virtual void Exit() { }

        public virtual void TransitionTo(string newStateName)
        {
            this.machine.Transition(newStateName.ToLower());
        }

    }
}
