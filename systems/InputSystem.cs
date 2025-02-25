// InputSystem.cs
using Godot;
using System;
using System.Collections.Generic;

namespace systems
{
    [GlobalClass]
    public partial class InputSystem : core.BaseSystem
    {

        public InputSystem()
        {

            requiredComponents.Add(typeof(components.InputComponent));
            requiredComponents.Add(typeof(components.PositionComponent)); // we don't modify this, but controlling an entity without position doesn't make sense.
            requiredComponents.Add(typeof(components.VelocityComponent));
        }

        public override void _Ready()
        {
            base._Ready();
            GD.Print("InputSystem Ready");
        }

        public override void _Process(double delta)
        {
            base._Process(delta);

            List<Node> entities = GetEntities();

            // player can only control one at a time.
            Node controlledEntity = entities[0];


            components.InputComponent input = _entityManager.GetComponent<components.InputComponent>(controlledEntity);
            components.VelocityComponent vel = _entityManager.GetComponent<components.VelocityComponent>(controlledEntity);

            vel.Velocity = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

        }
    }
}
