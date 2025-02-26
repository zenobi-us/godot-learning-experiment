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

        public static BehaviourStatus SetTargetPosition(
            BehaviourTreeBlackboardContext context,
            Godot.Vector2 position
        )
        {

            if (context.EntityManager.HasComponent<TargetPositionComponent>(context.Entity))
            {
                context.EntityManager.GetComponent<TargetPositionComponent>(context.Entity).Position = position;
                return BehaviourStatus.Succeeded;
            }

            context.EntityManager.AddComponent(context.Entity, new TargetPositionComponent(position));


            return BehaviourStatus.Succeeded;
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
            PositionComponent positionComponent = context.EntityManager.GetComponent<PositionComponent>(context.Entity);
            VelocityComponent velocityComponent = context.EntityManager.GetComponent<VelocityComponent>(context.Entity);
            TargetPositionComponent targetComponent = context.EntityManager.GetComponent<TargetPositionComponent>(context.Entity);

            if (velocityComponent == null)
            {
                GD.Print("No velocity component");
                return BehaviourStatus.Failed;
            }

            if (positionComponent == null || targetComponent == null)
            {
                GD.Print("No position or target component");
                velocityComponent.Velocity = Godot.Vector2.Zero;
                return BehaviourStatus.Failed;
            }

            var direction = positionComponent.Position.DirectionTo(targetComponent.Position).Normalized();
            velocityComponent.Velocity = new Godot.Vector2(
                direction.X,
                direction.Y
            );
            return BehaviourStatus.Succeeded; // Still moving towards the target
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
