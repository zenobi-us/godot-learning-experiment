using behaviours;
using BehaviourTree;
using BehaviourTree.FluentBuilder;
using Godot;

namespace core
{

    public abstract partial class BehaviourTreeResource : Resource
    {
        public abstract IBehaviour<BehaviourTreeBlackboardContext> GetTree();
    }
}
