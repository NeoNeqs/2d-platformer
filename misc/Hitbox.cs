using Godot;

namespace Platformer;

public partial class Hitbox : Area2D {

    private void OnHitboxEntered(Node2D node) {
        if (node is not Player player) {
            return;
        }

        player.Die();
    }
}
