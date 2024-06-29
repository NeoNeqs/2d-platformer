using Godot;

namespace Platformer;

public partial class Coin : Area2D {
    public override void _Ready() {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("default");
    }

    private void OnBodyEntered(Node2D node) {
        if (node is not Player player) {
            return;
        }

        player.AddCoin();
        QueueFree();
    }
}