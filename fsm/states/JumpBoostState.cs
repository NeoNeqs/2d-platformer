using Godot;

namespace Platformer;

[GlobalClass]
public partial class JumpBoostState : State {
    private Vector2 _newVelocity;

    public override void EnterState() {
        base.EnterState();
        GetStateMachine().Player.GetJumpPad()?.Spring();
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);

        Player player = GetStateMachine().Player;
        if (!player.IsOnJumpPad()) {
            GetStateMachine().ChangeState<MovementState>();
        }

        _newVelocity = player.Velocity;

        _newVelocity.Y = -400;
        player.Velocity = _newVelocity;
        player.MoveAndSlide();
    }
}