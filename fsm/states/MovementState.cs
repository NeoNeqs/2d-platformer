using Godot;

namespace Platformer;

[GlobalClass]
public partial class MovementState : State {
    private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    private Vector2 _newVelocity;
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        
        Player player = GetStateMachine().Player;

        _newVelocity = player.Velocity;
        float direction = HandleMovement(player, (float)delta);

        player.Velocity = _newVelocity;
        player.MoveAndSlide();
        
        HandlePushing(player, direction, (float)delta);
    }

    private void ApplyGravity(float delta) {
        _newVelocity.Y += _gravity * delta - 8;
    }

    private float HandleMovement(Player player, float delta) {
        if (player.IsOnLadder()) {
            GetStateMachine().ChangeState<ClimbingState>();
        } else if (player.IsOnJumpPad()) {
            GetStateMachine().ChangeState<JumpBoostState>();
        }
        
        float direction = Input.GetAxis("left", "right");
        if (direction != 0) {
            _newVelocity.X = Mathf.MoveToward(_newVelocity.X, player.Speed * direction, 40);
            player.Sprite.Play("Run");
            player.Sprite.FlipH = direction > 0;
        } else {
            _newVelocity.X = Mathf.MoveToward(_newVelocity.X, 0, player.Speed);
            player.Sprite.Play("Idle");
        }
        
        if (player.IsOnFloor()) {
            if (Input.IsActionJustPressed("ui_accept")) {
                _newVelocity.Y = player.JumpVelocity;
            }
        } else {
            ApplyGravity(delta);
            
            player.Sprite.Play("Jump");
        }

        return direction;
    }

    private static void HandlePushing(Player player, float direction, float delta) {
        int collisions = player.GetSlideCollisionCount();
        for (var i = 0; i < collisions; i++) {
            KinematicCollision2D collision = player.GetSlideCollision(i);

            if (collision.GetAngle() > 0 && collision.GetCollider() is MoveableBox box) {
                box.Push(direction * 6000 * delta);
            }
        }
    }
}