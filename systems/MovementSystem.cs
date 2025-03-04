using components;
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

                PositionComponent position = _entityManager.GetComponent<PositionComponent>(entity);
                VelocityComponent velocity = _entityManager.GetComponent<VelocityComponent>(entity);
                if (position == null)
                {
                    continue;
                }

                if (velocity.Velocity == Vector2.Zero)
                {
                    continue;
                }

                Move(
                    entity.Name,
                    characterBody2D,
                    velocity,
                    position
                );

            }
        }

        private void Move(
            string Name,
            CharacterBody2D characterBody2D,
            VelocityComponent velocityComponent,
            PositionComponent positionComponent
        )
        {

            var movement = new Vector2(
                (float)velocityComponent.MAX_SPEED * velocityComponent.Velocity.X,
                (float)velocityComponent.MAX_SPEED * velocityComponent.Velocity.Y
            );
            var originalPosition = characterBody2D.Position;


            // move and slide the entity based on its velocity and acceleration
            characterBody2D.Velocity = movement;

            characterBody2D.MoveAndSlide();

            // update the component
            positionComponent.Position = new Vector2(
                characterBody2D.Position.X,
                characterBody2D.Position.Y
            );

            _eventManager.Publish(
                new events.EntityMovedEvent(
                    Name,
                    originalPosition.X,
                    originalPosition.Y,
                    characterBody2D.Position.X,
                    characterBody2D.Position.Y
                )
            );
        }
    }
}
