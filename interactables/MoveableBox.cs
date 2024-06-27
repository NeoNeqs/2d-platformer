using Godot;

public partial class MoveableBox : RigidBody2D {

    private void OnBodyEntered(Node node) {
        GD.Print($"Enter: {node}");
    }

    private void OnBodyExited(Node node) {
        GD.Print($"Exit: {node}");
    }
}