using System;
using System.Linq;
using BehaviourTree;
using components;
using Godot;

namespace behaviours
{

    public static class BehaviourActions
    {

        public static BehaviourStatus SetEntityAsTarget(BehaviourTreeBlackboardContext context, string targetEntityId, int threshold = 40)
        {
            var targetEntity = context.EntityManager.GetEntitiesWithComponents(typeof(NameableComponent))
                .Where(entity =>
                {
                    var entityComponents = context.EntityManager.GetComponents<NameableComponent>(entity);
                    var isNamed = entityComponents.Any(component => component.Id == targetEntityId);
                    return isNamed;
                })
                .FirstOrDefault();

            var targetEntityComponent = context.EntityManager.GetComponent<TargetEntityComponent>(context.Entity, targetEntityId);

            // target doesn't exist
            if (targetEntity == null)
            {
                return BehaviourStatus.Failed;
            }


            if (targetEntityComponent == null)
            {
                context.EntityManager.AddComponent(context.Entity, new TargetEntityComponent(targetEntity, targetEntityId, threshold));
                return BehaviourStatus.Succeeded;
            }

            targetEntityComponent.Id = targetEntityId;
            targetEntityComponent.TargetEntity = targetEntity;

            return BehaviourStatus.Succeeded;
        }

        public static Func<BehaviourTreeBlackboardContext, BehaviourStatus> SetEntityAsTargetFactory(string targetEntityId, int threshold = 40)
        {
            return (context) =>
                {
                    var result = SetEntityAsTarget(context, targetEntityId, threshold);
                    return result;
                };
        }

        public static Func<BehaviourTreeBlackboardContext, BehaviourStatus> SetTargetPosition(
            string targetPositionId,
            Func<BehaviourTreeBlackboardContext, Godot.Vector2> getPositionFunc,
            int threshold = 40
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
                        return BehaviourStatus.Succeeded;
                    }

                    ctx.EntityManager.AddComponent(ctx.Entity, new TargetPositionComponent(targetPositionId, position, threshold));
                    return BehaviourStatus.Succeeded;
                }
                catch
                {
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

        public static Func<BehaviourTreeBlackboardContext, BehaviourStatus> SetRandomNearbyTargetPosition(string targetId, int radius, int threshold = 40)
        {
            return (context) =>
            {
                var positionComponent = context.EntityManager.GetComponent<PositionComponent>(context.Entity);

                if (positionComponent == null)
                {
                    return BehaviourStatus.Failed;
                }

                var targetPositionComponent = context.EntityManager.GetComponent<TargetPositionComponent>(context.Entity, targetId);

                if (targetPositionComponent != null && positionComponent.Position.DistanceTo(targetPositionComponent.Position) < targetPositionComponent.Threshold)
                {
                    context.EntityManager.RemoveComponent<TargetPositionComponent>(context.Entity, targetId);
                    return BehaviourStatus.Succeeded;
                }

                try
                {
                    var max = Math.PI * 2;
                    var min = 0;
                    var randomDirection = new Random().NextDouble() * (max - min) + min;
                    var randomDistance = new Random().Next(0, radius);
                    var targetPosition = new Godot.Vector2(
                        positionComponent.Position.X + randomDistance * (float)Math.Cos(randomDirection),
                        positionComponent.Position.Y + randomDistance * (float)Math.Sin(randomDirection)

                    );
                    if (targetPositionComponent == null)
                    {
                        context.EntityManager.AddComponent(context.Entity, new TargetPositionComponent(targetId, targetPosition, threshold));
                        return BehaviourStatus.Succeeded;
                    }

                    targetPositionComponent.Position = targetPosition;

                    return BehaviourStatus.Succeeded;
                }
                catch
                {
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
                var targetPositionComponent = context.EntityManager.GetComponent<TargetPositionComponent>(context.Entity, targetId);

                if (targetPositionComponent == null)
                {
                    return BehaviourStatus.Failed;
                }


                return MoveToTargetPosition(context, targetPositionComponent.Position);
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
                return BehaviourStatus.Failed;
            }

            if (positionComponent == null)
            {
                velocityComponent.Velocity = Godot.Vector2.Zero;
                return BehaviourStatus.Failed;
            }

            var direction = positionComponent.Position.DirectionTo(targetPosition).Normalized();
            velocityComponent.Velocity = new Godot.Vector2(
                direction.X,
                direction.Y
            );

            if (positionComponent.Position.DistanceTo(targetPosition) > 2)
            {
                return BehaviourStatus.Failed;
            }

            return BehaviourStatus.Succeeded; // Still moving towards the target

        }

    }
}
