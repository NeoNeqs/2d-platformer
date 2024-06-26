using Godot;

namespace Platformer;

public partial class Spike : Area2D  {

    private void OnBodyEntered(Node2D node) {
        if (node is not Player) {
            return;
        }
        
        node.QueueFree();
    }
}
