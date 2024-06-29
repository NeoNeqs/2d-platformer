using Godot;

namespace Platformer;

public partial class Checkpoint : Area2D {
    private void OnBodyEntered(Node2D body) {
        if (body is not Player player) {
            return;
        }

        player.CheckPoint();
        SetDeferred("monitoring", false);
        GetNode<Node2D>("Flag").Visible = true;
    }
}