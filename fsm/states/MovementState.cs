using Godot;

namespace Platformer;

[GlobalClass]
public partial class MovementState : State {
    [Export] private Player _player;

    private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    private Vector2 _newVelocity;
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        
        _newVelocity = _player.Velocity;

        float direction = HandleMovement((float)delta);

        _player.Velocity = _newVelocity;
        _player.MoveAndSlide();
        HandlePushing(direction, (float)delta);
    }

    private void ApplyGravity(float delta) {
        _newVelocity.Y += _gravity * delta - 8;
    }

    private float HandleMovement(float delta) {
        if (_player.IsOnLadder()) {
            GetStateMachine().ChangeState<ClimbingState>();
        }
        
        float direction = Input.GetAxis("left", "right");
        if (direction != 0) {
            _newVelocity.X = Mathf.MoveToward(_newVelocity.X, _player.Speed * direction, 40);
            _player.Sprite.Play("Run");
            _player.Sprite.FlipH = direction > 0;
        } else {
            _newVelocity.X = Mathf.MoveToward(_newVelocity.X, 0, _player.Speed);
            _player.Sprite.Play("Idle");
        }
        
        if (_player.IsOnFloor()) {
            if (Input.IsActionJustPressed("ui_accept")) {
                _newVelocity.Y = _player.JumpVelocity;
            }
        } else {
            ApplyGravity(delta);
            
            if (Input.IsActionJustPressed("ui_accept") && _newVelocity.Y < _player.MinJumpVelocity) {
                _newVelocity.Y = _player.MinJumpVelocity;
            }
            _player.Sprite.Play("Jump");
        }

        return direction;
    }

    private void HandlePushing(float direction, float delta) {
        int collisions = _player.GetSlideCollisionCount();
        for (var i = 0; i < collisions; i++) {
            KinematicCollision2D collision = _player.GetSlideCollision(i);

            if (collision.GetAngle() > 0 && collision.GetCollider() is MoveableBox box) {
                box.Push(direction * 6000 * delta);
            }
        }
    }
}