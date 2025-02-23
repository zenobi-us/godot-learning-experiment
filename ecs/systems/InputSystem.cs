// InputSystem.cs
using Godot;
using System;
using System.Collections.Generic;

namespace systems
{

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

            // // Reset velocity
            // vel.Velocity = new Vector2(0, 0);

            // // Check keyboard input
            // if (Input.IsActionPressed("ui_right"))
            // {
            //     vel.Velocity = new Vector2(input.Speed, 0);
            // }
            // if (Input.IsActionPressed("ui_left"))
            // {
            //     vel.Velocity = new Vector2(-input.Speed, 0);
            // }
            // if (Input.IsActionPressed("ui_down"))
            // {
            //     vel.Velocity = new Vector2(0, input.Speed);
            // }
            // if (Input.IsActionPressed("ui_up"))
            // {
            //     vel.Velocity = new Vector2(0, -input.Speed);
            // }
        }
    }
}
