using Godot;
using System;
using System.Collections.Generic;

namespace systems
{
    [GlobalClass]
    public partial class BehaviourTreeSystem : core.BaseSystem
    {
        public BehaviourTreeSystem()
        {
            requiredComponents.Add(typeof(components.BehaviourTreeComponent));

        }

        public override void _Ready()
        {
            base._Ready();
            GD.Print("Behavior Tree System ready");
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);

            List<Node> entities = GetEntities();

            foreach (Node entity in entities)
            {
                components.BehaviourTreeComponent btComponent = _entityManager.GetComponent<components.BehaviourTreeComponent>(entity);
                if (btComponent == null || btComponent.Behaviour == null)
                {
                    continue;
                }

                var mousePosition = GetViewport().GetMousePosition();

                behaviours.BehaviourTreeBlackboardContext context = new behaviours.BehaviourTreeBlackboardContext(
                    entity,
                    _entityManager,
                    mousePosition,
                    (long)Time.GetUnixTimeFromSystem()
                );

                btComponent.Behaviour.Tick(context);
            }
        }
    }
}
