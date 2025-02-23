using Godot;
using System;
using System.Collections.Generic;

namespace systems
{
    // Example movement system
    public partial class MovementSystem : core.BaseSystem

    {
        [Export]
        public Godot.NodePath EventManagerPath { get; set; }
        private core.EventManager _eventManager;


        public MovementSystem()
        {
            requiredComponents.Add(typeof(components.PositionComponent));
            requiredComponents.Add(typeof(components.VelocityComponent));
        }


        public override void _Ready()
        {
            base._Ready();
            _eventManager = GetNode<core.EventManager>(EventManagerPath);

            GD.Print("MovementSystem Ready");
        }

        /**
         * A method that takes an delta, vector2 input, and a character body2d as input.
          returns a vector2
         */
        public Vector2 AcceleterateEntity(
            double delta,
            components.PositionComponent position,
            components.VelocityComponent velocity
        )
        {

            float speed = (float)velocity.ACCELERATION * (float)delta;

            double x = Mathf.MoveToward(
                position.Position.X,
                velocity.MAX_SPEED * velocity.Velocity.X,
                speed
            );

            double y = Mathf.MoveToward(
                position.Position.Y,
                velocity.MAX_SPEED * velocity.Velocity.Y,
                speed
            );

            return new Vector2((float)x, (float)y);
        }

        public override void _Process(double delta)
        {
            base._Process(delta);

            List<Node> entities = GetEntities();

            foreach (var entity in entities)
            {

                // ignore it if it is not a CharacterBody2D.
                if (!(entity is Godot.CharacterBody2D characterBody2D))
                {
                    GD.PrintErr("Entity is not a CharacterBody2D. Skipping...", entity);
                    continue;
                }

                components.PositionComponent position = _entityManager.GetComponent<components.PositionComponent>(entity);
                components.VelocityComponent velocity = _entityManager.GetComponent<components.VelocityComponent>(entity);
                if (position == null)
                {
                    continue;
                }

                if (velocity.Velocity == Vector2.Zero)
                {
                    continue;
                }

                var movement = AcceleterateEntity(
                    delta,
                    position,
                    velocity
                );
                var originalPosition = characterBody2D.Position;

                // update the component
                position.Position = movement;

                // move and slide the entity based on its velocity and acceleration
                characterBody2D.Velocity = movement;

                characterBody2D.MoveAndSlide();

                _eventManager.Publish(
                    new events.EntityMovedEvent(
                        entity.Name,
                        originalPosition.X,
                        originalPosition.Y,
                        characterBody2D.Position.X,
                        characterBody2D.Position.Y
                    )
                );

            }
        }
    }
}
