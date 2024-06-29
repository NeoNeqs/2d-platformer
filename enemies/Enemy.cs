using Godot;

namespace Platformer;

public partial class Enemy : CharacterBody2D {
    private int _speed = 120;
    private Vector2 _direction = Vector2.Right;

    private RayCast2D _leftLedgeCheck;
    private RayCast2D _rightLedgeCheck;
    private AnimatedSprite2D _sprite;
    
    private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready() {
        _leftLedgeCheck = GetNode<RayCast2D>("LeftLedgeCheck");
        _rightLedgeCheck = GetNode<RayCast2D>("RightLedgeCheck");
        _sprite = GetNode<AnimatedSprite2D>("Sprite");
    }

    public override void _PhysicsProcess(double delta) {
        if (IsOnWall() || !_leftLedgeCheck.IsColliding() || !_rightLedgeCheck.IsColliding()) {
            _direction *= -1;
            _sprite.FlipH = _direction.X > 0;
        }

        if (!IsOnFloor()) {
            Velocity += Vector2.Down * _gravity * (float)delta;
        } else {
            Velocity = _direction * _speed;
        }
        
        MoveAndSlide();
    }
}