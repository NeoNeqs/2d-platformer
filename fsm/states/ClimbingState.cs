using Godot;

namespace Platformer;

[GlobalClass]
public partial class ClimbingState : State {
    
    private Vector2 _newVelocity;
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        
        Player player = GetStateMachine().Player;
        
        _newVelocity = player.Velocity;

        Vector2 dir = Input.GetVector("left", "right", "up", "down");
        HandleClimbing(player, dir);
        
        player.Velocity = _newVelocity;
        player.MoveAndSlide();
    }

    private void HandleClimbing(Player player, Vector2 dir) {
        if (!player.IsOnLadder()) {
            GetStateMachine().ChangeState<MovementState>();
        }
        
        if (dir.X != 0) {
            _newVelocity.X = Mathf.MoveToward(_newVelocity.X, player.Speed * dir.X, 40);
            player.Sprite.Play("Run");
            player.Sprite.FlipH = dir.X > 0;
        } else {
            _newVelocity.X = Mathf.MoveToward(_newVelocity.X, 0, player.Speed);
            player.Sprite.Play("Idle");
        }

        _newVelocity.Y = dir.Y * 60;
    }
}
