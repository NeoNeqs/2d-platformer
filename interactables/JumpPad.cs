using Godot;

namespace Platformer;

public partial class JumpPad : Area2D {
    public void Spring() {
        GetNode<AnimatedSprite2D>("Sprite").Play("Spring");
    }
}