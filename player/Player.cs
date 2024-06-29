using System;
using Godot;

namespace Platformer;

public partial class Player : CharacterBody2D {
    private const float Speed = 120.0f;
    private const float JumpVelocity = -200.0f;
    private const float MinJumpVelocity = -70f;
    private ushort _currentLevel = 1;

    private Vector2 _initialPosition;
    private ushort _coins;
    private Key _followKey;
    private AnimatedSprite2D _sprite;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready() {
        CheckPoint();
        _sprite = GetNode<AnimatedSprite2D>("Sprite");
    }

    public override void _PhysicsProcess(double delta) {
        Vector2 velocity = Velocity;

        if (IsOnFloor()) {
            if (Input.IsActionJustPressed("ui_accept")) {
                velocity.Y = JumpVelocity;
            }
        } else {
            _sprite.Play("Jump");
            velocity.Y += (_gravity * (float)delta) - 8;
            if (Input.IsActionJustPressed("ui_accept") && velocity.Y < MinJumpVelocity) {
                velocity.Y = MinJumpVelocity;
            }
        }

        // Get the input direction and handle the movement/deceleration.
        float direction = Input.GetAxis("left", "right");
        if (direction != 0) {
            velocity.X = Mathf.MoveToward(Velocity.X, Speed * direction, 40);
            _sprite.Play("Run");
            _sprite.FlipH = direction > 0;
        } else {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            _sprite.Play("Idle");
        }

        Velocity = velocity;
        MoveAndSlide();

        int collisions = GetSlideCollisionCount();
        for (var i = 0; i < collisions; i++) {
            KinematicCollision2D collision = GetSlideCollision(i);

            if (collision.GetAngle() > 0 && collision.GetCollider() is MoveableBox box) {
                box.Push(direction * 6000 * (float)delta);
            }
        }


        int lookDir = Mathf.Sign(Convert.ToInt32(!_sprite.FlipH) - 0.5f);
        if (_followKey is not null) {
            _followKey.GlobalPosition =
                _followKey.GlobalPosition.Lerp(
                    GlobalPosition + new Vector2(lookDir * 20, 0), 0.2f);
        }
    }

    public override void _Input(InputEvent @event) {
        if (@event is not InputEventKey) return;
        if (@event.IsActionPressed("reset")) {
            Reset();
        }
    }

    public void Die() {
        GlobalPosition = _initialPosition;
    }

    private void Reset() {
        GetTree().CallDeferred("reload_current_scene");
    }

    public void AdvanceLevel() {
        if (_followKey is null) return;

        _currentLevel++;
        GetTree().CallDeferred("change_scene_to_file", $"res://levels/level{_currentLevel}.tscn");
    }

    public void CheckPoint() {
        _initialPosition = GlobalPosition;
    }

    public void AddCoin() {
        _coins += 1;
        SignalBus.Instance.EmitCoinCollected(_coins);
    }

    public void AttachKey(Key key) {
        _followKey = key;
    }
}