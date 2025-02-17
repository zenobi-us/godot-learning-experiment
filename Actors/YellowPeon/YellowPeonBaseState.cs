namespace Actors
{
    public partial class YellowPeonBaseState : Components.AnimatedMoveableState
    {
        public bool IsMoving()
        {
            return this.Moveable.Velocity.X != 0 || this.Moveable.Velocity.Y != 0;
        }


    }
}
