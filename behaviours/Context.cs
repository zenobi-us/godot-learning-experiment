using Godot;

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

        public BehaviourTreeBlackboardContext(
            Node entity,
            core.EntityManager entityManager,
            long timeStampInMilliseconds)
        {
            _timeStampInMilliseconds = timeStampInMilliseconds;
            Entity = entity;
            EntityManager = entityManager;
        }

        public long GetTimeStampInMilliseconds()
        {
            return _timeStampInMilliseconds;
        }

    }
}
