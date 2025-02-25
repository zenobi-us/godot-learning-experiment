using System.Numerics;
using BehaviourTree;
using components;
using Godot;

namespace behaviours
{

    public static class BehaviourActions
    {

        public static BehaviourStatus SetEntityAsTarget(
            behaviours.BehaviourTreeBlackboardContext context,
            Node target
        )
        {

            if (context.EntityManager.HasComponent<components.TargetEntityComponent>(context.Entity))
            {
                context.EntityManager.GetComponent<components.TargetEntityComponent>(context.Entity).TargetEntity = target;
                return BehaviourStatus.Succeeded;
            }

            context.EntityManager.AddComponent(context.Entity, new components.TargetEntityComponent(target));


            return BehaviourStatus.Succeeded;
        }

        public static BehaviourStatus SetTargetPosition(
            behaviours.BehaviourTreeBlackboardContext context,
            Godot.Vector2 position
        )
        {


            if (context.EntityManager.HasComponent<components.TargetPositionComponent>(context.Entity))
            {
                context.EntityManager.GetComponent<components.TargetPositionComponent>(context.Entity).Position = position;
                return BehaviourStatus.Succeeded;
            }

            context.EntityManager.AddComponent(context.Entity, new components.TargetPositionComponent(position));


            return BehaviourStatus.Succeeded;
        }

        public static BehaviourStatus MoveToTargetEntityPosition(
            behaviours.BehaviourTreeBlackboardContext context
        )
        {
            return BehaviourStatus.Failed;
            // components.PositionComponent position = context.EntityManager.GetComponent<components.PositionComponent>(context.Entity);
            // components.VelocityComponent velocity = context.EntityManager.GetComponent<components.VelocityComponent>(context.Entity);
            // components.TargetEntityComponent target = context.EntityManager.GetComponent<components.TargetEntityComponent>(context.Entity);

            // if (velocity == null)
            // {
            //     return BehaviourStatus.Failed;
            // }

            // if (position == null || target == null)
            // {
            //     velocity.Velocity = Godot.Vector2.Zero;
            //     return BehaviourStatus.Failed;
            // }

            // var targetPosition = context.EntityManager.GetComponent<components.PositionComponent>(target.TargetEntity);


            // return BehaviourStatus.Running; // Still moving towards the target
        }

        public static BehaviourStatus MoveToTargetPosition(
            behaviours.BehaviourTreeBlackboardContext context
        )
        {
            components.PositionComponent positionComponent = context.EntityManager.GetComponent<components.PositionComponent>(context.Entity);
            components.VelocityComponent velocityComponent = context.EntityManager.GetComponent<components.VelocityComponent>(context.Entity);
            components.TargetPositionComponent targetComponent = context.EntityManager.GetComponent<components.TargetPositionComponent>(context.Entity);

            if (velocityComponent == null)
            {
                return BehaviourStatus.Failed;
            }

            if (positionComponent == null || targetComponent == null)
            {
                velocityComponent.Velocity = Godot.Vector2.Zero;
                return BehaviourStatus.Failed;
            }

            var distance = positionComponent.Position.DistanceTo(targetComponent.Position);

            if (distance < 2)
            {
                velocityComponent.Velocity = Godot.Vector2.Zero;
                return BehaviourStatus.Succeeded;
            }

            var direction = positionComponent.Position.DirectionTo(targetComponent.Position);
            var velocity = direction.Normalized() * 4;

            velocityComponent.Velocity = velocity;
            return BehaviourStatus.Running; // Still moving towards the target
        }
    }
}
