using Godot;
using Platformer;

public partial class Key : Area2D {

    private void OnBodyEntered(Node2D body) {
        if (body is not Player player) return;
        
        SetDeferred("monitoring", false);
        player.AttachKey(this);
    }
}