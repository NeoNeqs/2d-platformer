using Godot;
using System;
using Platformer;

public partial class ExitDoor : Area2D {

    private void OnBodyEntered(Node2D node) {
        if (node is not Player player) {
            return;
        }

        player.AdvanceLevel();
    }
}