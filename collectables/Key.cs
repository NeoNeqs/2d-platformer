using System;
using Godot;

namespace Platformer;

public partial class Key : Area2D {
    private Player _player;

    private void OnBodyEntered(Node2D body) {
        if (body is not Player player) {
            return;
        }

        SetDeferred("monitoring", false);
        player.Level.Unlock();
        
        _player = player;
    }

    public override void _PhysicsProcess(double delta) {
        if (_player is null) {
            return;
        }
    
        FollowPlayer((float)delta);
    }

    private void FollowPlayer(float delta) {
        int lookDir = Mathf.Sign(Convert.ToInt32(!_player.Sprite.FlipH) - 0.5f);
        GlobalPosition = GlobalPosition.Decay(_player.GlobalPosition + new Vector2(lookDir * 20, 0), 8, delta);
    }
}