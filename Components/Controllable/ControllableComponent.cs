using System;
using System.Diagnostics;
using System.Numerics;
using Core;
using Godot;

namespace Components
{
  public partial class ControllableComponent : Component
  {

    public ControllableComponent()
    {
      this.ComponentName = "ControllableComponent";
    }

    private MoveableComponent moveable { get; set; }

    public override void _Ready()
    {

      this.moveable = Core.ComponentSearch.GetComponent<MoveableComponent>(
        this.GetParent(),
        "MoveableComponent"
      );
      Debug.Assert(this.moveable != null, "ControllableComponent.no_moveable");

    }


    public override void _Process(double delta)
    {
      this.Update(delta);
    }

    public void Update(double delta)
    {

      if (this.moveable == null)
      {
        return;
      }

      this.moveable.input = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

      // this.isJumpJustPressed = godot.Input.is_action_just_pressed("ui_jump");
      // this.jumpPressing = godot.Input.is_action_pressed("ui_jump");
    }
  }
}

//   @property({})
//   jumpPressing: boolean = false;

//   @property({})
//   get isJumpJustPressed() {
//     return this._isJumpJustPressed;
//   }
//   set isJumpJustPressed(value: boolean) {
//     this._isJumpJustPressed = value;
//     if (!this.jumpPressedBufferTimer) {
//       return;
//     }

//     if (value) {
//       this.jumpPressedBufferTimer.start();
//     }
//   }
//   private _isJumpJustPressed: boolean;

//   @property({})
//   get jumpPressBuffer() {
//     return !this.jumpPressedBufferTimer.is_stopped();
//   }

//   set jumpPressBuffer(value: boolean) {
//     if (!value) {
//       this.jumpPressedBufferTimer.stop();
//     }
//     this.jumpPressedBufferTimer.start();
//   }

// moveable: MoveableComponent = null;

// _process(): void {
//     this.update();
//   }

//   update() {
//   if (!this.moveable)
//   {
//     console.log("ControllableComponent.no_moveable");
//     return;
//   }
//   const x = godot.Input.get_axis("ui_left", "ui_right");
//   const y = godot.Input.get_axis("ui_up", "ui_down");

//   this.moveable.input.x = x;
//   this.moveable.input.y = y;

//   // this.isJumpJustPressed = godot.Input.is_action_just_pressed("ui_jump");
//   // this.jumpPressing = godot.Input.is_action_pressed("ui_jump");
// }
// }

// export default ControllableComponent;
