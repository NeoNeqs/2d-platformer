using Godot;

namespace Platformer;

[GlobalClass]
public partial class ClimbingState : State {
    [Export] private Player _player;
    
    private Vector2 _newVelocity;
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        
        _newVelocity = _player.Velocity;

        Vector2 dir = Input.GetVector("left", "right", "up", "down");
        HandleClimbing(dir);
        
        _player.Velocity = _newVelocity;
        _player.MoveAndSlide();
    }

    private void HandleClimbing(Vector2 dir) {
        if (!_player.IsOnLadder()) {
            GetStateMachine().ChangeState<MovementState>();
        }
        
        if (dir.X != 0) {
            _newVelocity.X = Mathf.MoveToward(_newVelocity.X, _player.Speed * dir.X, 40);
            _player.Sprite.Play("Run");
            _player.Sprite.FlipH = dir.X > 0;
        } else {
            _newVelocity.X = Mathf.MoveToward(_newVelocity.X, 0, _player.Speed);
            _player.Sprite.Play("Idle");
        }

        _newVelocity.Y = dir.Y * 60;
    }
}
