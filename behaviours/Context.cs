using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace behaviours
{
    public sealed class BehaviourTreeBlackboardContext : BehaviourTree.IClock
    {
        /**
         * A sense of time. when is "it".
         */
        private readonly long _timeStampInMilliseconds;

        /**
         * Current entity
         */
        public Node Entity { get; }

        /**
         * The current scene
         */
        public core.EntityManager EntityManager { get; }

        /*
         * Current mouse position
         */
        public Vector2 MousePosition { get; set; }


        public BehaviourTreeBlackboardContext(
            Node entity,
            core.EntityManager entityManager,
            Vector2 mousePosition,
            long timeStampInMilliseconds)
        {
            _timeStampInMilliseconds = timeStampInMilliseconds;
            Entity = entity;
            EntityManager = entityManager;
            MousePosition = mousePosition;
        }

        public long GetTimeStampInMilliseconds()
        {
            return _timeStampInMilliseconds;
        }

    }
}
