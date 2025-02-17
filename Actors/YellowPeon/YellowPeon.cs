using Godot;

namespace Actors
{
    public partial class YellowPeon : Objects.Actor
    {
        public override void _Ready()
        {
            base._Ready();
            SetName("Yellow Peon");
        }
    }
}
