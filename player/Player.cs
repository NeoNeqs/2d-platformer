using System.Diagnostics;
using Godot;

namespace Platformer;

public partial class Player : CharacterBody2D {
    [Export] public long Speed;
    [Export] public long JumpVelocity;
    [Export] public long MinJumpVelocity;

    private Vector2 _initialPosition;
    private ushort _collectedCoinsCount;

    public Level Level { get; private set; }
    public AnimatedSprite2D Sprite { get; private set; }
    public ShapeCast2D InteractablesShapeCast { get; private set; }

    private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready() {
        Debug.Assert(GetTree().CurrentScene is Level, "Player can only exist in a scene where the root is Level");
        
        Level = GetTree().CurrentScene as Level;
        InteractablesShapeCast = GetNode<ShapeCast2D>("InteractablesShapeCast");
        Sprite = GetNode<AnimatedSprite2D>("Sprite");
        
        CheckPoint();
    }

    public override void _Input(InputEvent @event) {
        if (@event is not InputEventKey) return;
        if (@event.IsActionPressed("reset")) {
            Level.Reset();
        }
    }

    public bool IsOnLadder() {
        if (!InteractablesShapeCast.IsColliding()) {
            return false;
        }

        if (InteractablesShapeCast.GetCollider(0) is not Ladder) {
            return false;
        }

        return true;
    }

    public bool IsOnJumpPad() {
        if (!InteractablesShapeCast.IsColliding()) {
            return false;
        }

        if (InteractablesShapeCast.GetCollider(0) is not JumpPad) {
            return false;
        }

        return true;
    }
    

    public JumpPad GetJumpPad() {
        return InteractablesShapeCast.GetCollider(0) as JumpPad;
    }

    public void Die() {
        GlobalPosition = _initialPosition;
    }

    public void CheckPoint() {
        _initialPosition = GlobalPosition;
    }

    public void AddCoin() {
        _collectedCoinsCount += 1;
        SignalBus.Instance.EmitCoinCollected(_collectedCoinsCount);
    }
}