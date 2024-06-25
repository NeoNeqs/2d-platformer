using Godot;

namespace Platformer;

public partial class Player : CharacterBody2D {
    private const float Speed = 120.0f;
    private const float JumpVelocity = -220.0f;
    private const float MinJumpVelocity = -70f;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _PhysicsProcess(double delta) {
        Vector2 velocity = Velocity;

        // Add the gravity.
        if (IsOnFloor()) {
            if (Input.IsActionPressed("ui_accept")) {
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
            GD.Print(GetNode<AnimatedSprite2D>("Sprite").Animation);
            GetNode<AnimatedSprite2D>("Sprite").FlipH = direction > 0;
        } else {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            GetNode<AnimatedSprite2D>("Sprite").Play("Idle");
        }

        Velocity = velocity;
        MoveAndSlide();
    }
}