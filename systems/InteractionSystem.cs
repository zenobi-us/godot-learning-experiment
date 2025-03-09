// InteractionSystem.cs
using Godot;
using System.Collections.Generic;

namespace systems
{

    public partial class InteractionSystem : core.BaseSystem
    {

        [Export]
        public Godot.NodePath EventManagerPath { get; set; }
        private core.EventManager _eventManager;

        public InteractionSystem()
        {
            requiredComponents.Add(typeof(components.InteractionComponent));
        }

        public override void _Ready()
        {
            base._Ready();

            _eventManager = GetNode<core.EventManager>(EventManagerPath);

            GD.Print("InteractionSystem ready");
        }

        public override void _Input(InputEvent @event)
        {
            List<Node> entities = GetEntities();



            foreach (Node entity in entities)
            {
                components.InteractionComponent interaction = _entityManager.GetComponent<components.InteractionComponent>(entity);

                if (!interaction.IsActive)
                {
                    continue;

                }

                if (interaction.CurrentState == null)
                {
                    continue;
                }

                if (!(interaction.CurrentState is components.InteractionStateComponent state))
                {
                    continue;
                }

                Node nextState = state.ProcessInput(@event);
                if (nextState != null && nextState != state)
                {
                    state.OnExit();
                    interaction.CurrentState = nextState;
                    if (nextState is components.InteractionStateComponent newState)
                    {
                        newState.OnEnter();
                    }
                }
            }
        }

        // Optional: Trigger interaction start with a key (e.g., "Interact" action)
        public override void _Process(double delta)
        {
            base._Process(delta);

            if (!Input.IsActionJustPressed("interact")) // Define "interact" in Input Map (e.g., Space key)
            {
                return;
            }

            List<Node> entities = GetEntities();

            foreach (Node entity in entities)
            {
                components.InteractionComponent interaction = _entityManager.GetComponent<components.InteractionComponent>(entity);

                if (interaction.IsActive)
                {
                    continue;
                }
                if (interaction.CurrentState == null)
                {
                    continue;
                }

                interaction.IsActive = true;
                _eventManager.Publish(new events.InteractionStartedEvent());
                if (interaction.CurrentState is components.InteractionStateComponent state)
                {
                    state.OnEnter();
                }
            }
        }
    }
}
