using System;
using System.Diagnostics;
using Godot;

namespace Platformer;

public partial class Player : CharacterBody2D {
    [Export] public long Speed;
    [Export] public long JumpVelocity;
    [Export] public long MinJumpVelocity;

    private Key _followKey;
    private Vector2 _initialPosition;
    private ushort _coins;
    private RayCast2D _ladderRay;

    public Level Level { get; private set; }
    public AnimatedSprite2D Sprite { get; private set; }

    private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready() {
        Debug.Assert(GetTree().CurrentScene is Level, "Player can only exist in a scene where the root is Level");
        Level = GetTree().CurrentScene as Level;
        
        Sprite = GetNode<AnimatedSprite2D>("Sprite");
        _ladderRay = GetNode<RayCast2D>("LadderRay");
        CheckPoint();
    }

    public override void _PhysicsProcess(double delta) {
        int lookDir = Mathf.Sign(Convert.ToInt32(!Sprite.FlipH) - 0.5f);
        if (_followKey is not null) {
            _followKey.GlobalPosition = _followKey.GlobalPosition.Decay(GlobalPosition + new Vector2(lookDir * 20, 0),
                8, (float)delta);
        }
    }
    
    public override void _Input(InputEvent @event) {
        if (@event is not InputEventKey) return;
        if (@event.IsActionPressed("reset")) {
            Level.Reset();
        }
    }

    public bool IsOnLadder() {
        if (!_ladderRay.IsColliding()) {
            return false;
        }

        GodotObject collider = _ladderRay.GetCollider();

        if (collider is not Ladder) {
            return false;
        }

        return true;
    }

    public void Die() {
        GlobalPosition = _initialPosition;
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