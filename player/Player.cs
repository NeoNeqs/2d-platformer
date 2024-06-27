using System;
using System.ComponentModel.Design;
using Godot;

namespace Platformer;

public partial class Player : CharacterBody2D {
    private const float Speed = 120.0f;
    private const float JumpVelocity = -200.0f;
    private const float MinJumpVelocity = -70f;
    private ushort _currentLevel = 1;

    private ushort _coins;
    private Key followKey;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _PhysicsProcess(double delta) {
        Vector2 velocity = Velocity;

        // Add the gravity.
        if (IsOnFloor()) {
            if (Input.IsActionJustPressed("ui_accept")) {
                velocity.Y = JumpVelocity;
            }
        } else {
            GetNode<AnimatedSprite2D>("Sprite").Play("Jump");
            velocity.Y += _gravity * (float)delta;
            if (Input.IsActionJustPressed("ui_accept") && velocity.Y < MinJumpVelocity) {
                velocity.Y = MinJumpVelocity;
            }

            velocity.Y -= 8;
        }

        // Get the input direction and handle the movement/deceleration.
        float direction = Input.GetAxis("left", "right");
        if (direction != 0) {
            velocity.X = Mathf.MoveToward(Velocity.X, Speed * direction, 40);
            GetNode<AnimatedSprite2D>("Sprite").Play("Run");
            GetNode<AnimatedSprite2D>("Sprite").FlipH = direction > 0;
        } else {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            GetNode<AnimatedSprite2D>("Sprite").Play("Idle");
        }

        Velocity = velocity;
        MoveAndSlide();
        
        int collisions = GetSlideCollisionCount();
        for (var i = 0; i < collisions; i++) {
            KinematicCollision2D collision = GetSlideCollision(i);

            if (collision.GetCollider() is MoveableBox body) {
                body.ApplyCentralImpulse(-collision.GetNormal() * 40000);
            }
        }
        bool lookDir = !GetNode<AnimatedSprite2D>("Sprite").FlipH;
        if (followKey is not null) {
            followKey.GlobalPosition =
                followKey.GlobalPosition.Lerp(
                    GlobalPosition + new Vector2(Mathf.Sign(Convert.ToInt32(lookDir) - 0.5f) * 20, 0), 0.3f);
        }
    }

    public void Die() {
        GetTree().CallDeferred("reload_current_scene");
    }

    public void AdvanceLevel() {
        _currentLevel++;
        GetTree().ChangeSceneToFile($"res://levels/level{_currentLevel}.tscn");
    }

    public void AddCoin() {
        _coins += 1;
        SignalBus.Instance.EmitCoinCollected(_coins);
    }

    public void AttachKey(Key key) {
        followKey = key;
    }
}