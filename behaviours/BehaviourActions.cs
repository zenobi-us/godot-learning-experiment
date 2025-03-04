using System;
using System.Linq;
using BehaviourTree;
using components;
using core;
using Godot;

namespace behaviours
{

    public static class BehaviourActions
    {

        public static BehaviourStatus SetEntityAsTarget(BehaviourTreeBlackboardContext context, string targetEntityId)
        {
            var targetEntity = context.EntityManager.GetEntitiesWithComponents(typeof(NameableComponent))
                .Where(entity =>
                {
                    return context.EntityManager.HasComponent<NameableComponent>(entity, targetEntityId);
                })
                .FirstOrDefault();
            var targetEntityComponent = context.EntityManager.GetComponent<TargetEntityComponent>(context.Entity, targetEntityId);

            // target doesn't exist
            if (targetEntity == null)
            {
                GD.Print($"No entity with NameableComponent.Id = {targetEntityId}");
                return BehaviourStatus.Failed;
            }


            if (targetEntityComponent == null)
            {
                context.EntityManager.AddComponent(context.Entity, new TargetEntityComponent(targetEntity, targetEntityId));
                GD.Print($"Created TargetEntityComponent for = {targetEntityId}");
                return BehaviourStatus.Succeeded;
            }

            targetEntityComponent.Id = targetEntityId;
            targetEntityComponent.TargetEntity = targetEntity;

            GD.Print($"Updated TargetEntityComponent for = {targetEntityId}");
            return BehaviourStatus.Succeeded;
        }

        public static Func<BehaviourTreeBlackboardContext, BehaviourStatus> SetEntityAsTargetFactory(string targetEntityId)
        {
            return (context) =>
                {
                    return SetEntityAsTarget(context, targetEntityId);
                };
        }

        public static Func<BehaviourTreeBlackboardContext, BehaviourStatus> SetTargetPosition(
            string targetPositionId,
            Func<BehaviourTreeBlackboardContext, Godot.Vector2> getPositionFunc
        )
        {
            return (ctx) =>
            {
                try
                {
                    var position = getPositionFunc(ctx);
                    var target = ctx.EntityManager.GetComponent<TargetPositionComponent>(ctx.Entity, targetPositionId);

                    // If target position already exists on entity, update the coordinates
                    if (target != null)
                    {
                        target.Position = position;
                        GD.Print($"Updated position for {targetPositionId} to {position}");
                        return BehaviourStatus.Succeeded;
                    }

                    ctx.EntityManager.AddComponent(ctx.Entity, new TargetPositionComponent(targetPositionId, position));
                    GD.Print($"Created target position {targetPositionId} at {position}");
                    return BehaviourStatus.Succeeded;
                }
                catch (Exception ex)
                {
                    GD.PrintErr($"something went wrong with {targetPositionId}");
                    GD.PrintErr(ex.ToString());
                    return BehaviourStatus.Failed;
                }
            };
        }

        public static Func<BehaviourTreeBlackboardContext, BehaviourStatus> RemoveTargetPosition(string targetPositionId)
        {
            return (ctx) =>
            {
                try
                {
                    ctx.EntityManager.RemoveComponent<TargetPositionComponent>(ctx.Entity, targetPositionId);
                    return BehaviourStatus.Succeeded;
                }
                catch
                {
                    return BehaviourStatus.Failed;
                }
            };
        }

        public static Func<BehaviourTreeBlackboardContext, BehaviourStatus> SetRandomNearbyTargetPosition(string targetId, int radius)
        {
            return (context) =>
            {
                var positionComponent = context.EntityManager.GetComponent<PositionComponent>(context.Entity);

                if (positionComponent == null)
                {
                    GD.Print("SetRandomNearbyTargetPosition: no position");
                    return BehaviourStatus.Failed;
                }

                try
                {
                    context.EntityManager.RemoveComponent<TargetPositionComponent>(context.Entity, targetId);
                    var max = Math.PI * 2;
                    var min = 0;
                    var randomDirection = new Random().NextDouble() * (max - min) + min;
                    var randomDistance = new Random().Next(0, radius);
                    var targetPosition = new Godot.Vector2(
                        positionComponent.Position.X + randomDistance * (float)Math.Cos(randomDirection),
                        positionComponent.Position.Y + randomDistance * (float)Math.Sin(randomDirection)

                    );
                    var patrolTarget = new TargetPositionComponent(targetId, targetPosition);
                    context.EntityManager.AddComponent(context.Entity, patrolTarget);
                    GD.Print($"SetRandomNearbyTargetPosition: Set random target position to {targetPosition}");

                    return BehaviourStatus.Succeeded;
                }
                catch (Exception ex)
                {
                    GD.Print("SetRandomNearbyTargetPosition: Failed to calculate a random target position", ex);
                    return BehaviourStatus.Failed;
                }
            };
        }


        public static BehaviourStatus MoveToTargetPositionFactory(
            BehaviourTreeBlackboardContext context
        )
        {
            return MoveToTargetPositionFactory("Default")(context);
        }

        public static Func<BehaviourTreeBlackboardContext, BehaviourStatus> MoveToTargetPositionFactory(
            string targetId
        )
        {
            return (context) =>
            {
                return MoveToTargetEntityPosition(context, targetId);
            };
        }

        public static BehaviourStatus MoveToTargetEntityPosition(
            BehaviourTreeBlackboardContext context,
            string targetId
        )
        {
            var targetEntityComponent = context.EntityManager.GetComponent<TargetEntityComponent>(context.Entity, targetId);

            if (targetEntityComponent == null)
            {
                return BehaviourStatus.Failed;
            }

            var targetPosition = context.EntityManager.GetComponent<PositionComponent>(targetEntityComponent.TargetEntity);
            return MoveToTargetPosition(context, targetPosition.Position);
        }

        public static BehaviourStatus MoveToTargetPosition(
            BehaviourTreeBlackboardContext context,
            Godot.Vector2 targetPosition
        )
        {

            var positionComponent = context.EntityManager.GetComponent<PositionComponent>(context.Entity);
            var velocityComponent = context.EntityManager.GetComponent<VelocityComponent>(context.Entity);

            if (velocityComponent == null)
            {
                GD.Print("No velocity component");
                return BehaviourStatus.Failed;
            }

            if (positionComponent == null)
            {
                GD.Print("No position component");
                velocityComponent.Velocity = Godot.Vector2.Zero;
                return BehaviourStatus.Failed;
            }

            var direction = positionComponent.Position.DirectionTo(targetPosition).Normalized();
            velocityComponent.Velocity = new Godot.Vector2(
                direction.X,
                direction.Y
            );

            if (positionComponent.Position.DistanceTo(targetPosition) < 2)
            {
                return BehaviourStatus.Succeeded; // Still moving towards the target
            }

            return BehaviourStatus.Running;
        }

    }
}
