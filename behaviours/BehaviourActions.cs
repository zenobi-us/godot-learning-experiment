using System;
using System.Linq;
using System.Numerics;
using BehaviourTree;
using components;
using Godot;

namespace behaviours
{

    public static class BehaviourActions
    {

        public static BehaviourStatus SetEntityAsTarget(
            BehaviourTreeBlackboardContext context,
            Node target
        )
        {

            if (context.EntityManager.HasComponent<TargetEntityComponent>(context.Entity))
            {
                context.EntityManager.GetComponent<TargetEntityComponent>(context.Entity).TargetEntity = target;
                return BehaviourStatus.Succeeded;
            }

            context.EntityManager.AddComponent(context.Entity, new TargetEntityComponent(target));


            return BehaviourStatus.Succeeded;
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

        public static BehaviourStatus MoveToTargetEntityPosition(
            BehaviourTreeBlackboardContext context
        )
        {
            return BehaviourStatus.Failed;
            // PositionComponent position = context.EntityManager.GetComponent<PositionComponent>(context.Entity);
            // VelocityComponent velocity = context.EntityManager.GetComponent<VelocityComponent>(context.Entity);
            // TargetEntityComponent target = context.EntityManager.GetComponent<TargetEntityComponent>(context.Entity);

            // if (velocity == null)
            // {
            //     return BehaviourStatus.Failed;
            // }

            // if (position == null || target == null)
            // {
            //     velocity.Velocity = Godot.Vector2.Zero;
            //     return BehaviourStatus.Failed;
            // }

            // var targetPosition = context.EntityManager.GetComponent<PositionComponent>(target.TargetEntity);


            // return BehaviourStatus.Running; // Still moving towards the target
        }

        public static BehaviourStatus MoveToTargetPosition(
            BehaviourTreeBlackboardContext context
        )
        {
            return MoveToTargetPosition("Default")(context);
        }

        public static Func<BehaviourTreeBlackboardContext, BehaviourStatus> MoveToTargetPosition(
            string targetId
        )
        {
            return (context) =>
            {
                var positionComponent = context.EntityManager.GetComponent<PositionComponent>(context.Entity);
                var velocityComponent = context.EntityManager.GetComponent<VelocityComponent>(context.Entity);
                var targetComponent = context.EntityManager.GetComponent<TargetPositionComponent>(context.Entity, targetId);

                if (velocityComponent == null)
                {
                    GD.Print("No velocity component");
                    return BehaviourStatus.Failed;
                }

                if (targetComponent == null)
                {
                    GD.Print($"No target component: {targetId}");
                    velocityComponent.Velocity = Godot.Vector2.Zero;
                    return BehaviourStatus.Failed;
                }


                if (positionComponent == null)
                {
                    GD.Print("No position component");
                    velocityComponent.Velocity = Godot.Vector2.Zero;
                    return BehaviourStatus.Failed;
                }

                var direction = positionComponent.Position.DirectionTo(targetComponent.Position).Normalized();
                velocityComponent.Velocity = new Godot.Vector2(
                    direction.X,
                    direction.Y
                );

                if (positionComponent.Position.DistanceTo(targetComponent.Position) < 2)
                {
                    return BehaviourStatus.Succeeded; // Still moving towards the target
                }

                return BehaviourStatus.Running;
            };
        }
        public static bool HasReachedTargetPosition(
            BehaviourTreeBlackboardContext context,
            int threshold = 20
        )
        {
            PositionComponent positionComponent = context.EntityManager.GetComponent<PositionComponent>(context.Entity);
            TargetPositionComponent targetComponent = context.EntityManager.GetComponent<TargetPositionComponent>(context.Entity);

            var distance = positionComponent.Position.DistanceTo(targetComponent.Position);

            return distance < threshold;
        }
    }
}
