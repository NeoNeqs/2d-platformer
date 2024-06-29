using Godot;

public partial class MoveableBox : CharacterBody2D {

    private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _PhysicsProcess(double delta) {
        Vector2 velocity = Velocity;
        
        if (!IsOnFloor()) {
            velocity.Y += _gravity * (float)delta;
        }

        Velocity = velocity;

        MoveAndSlide();
        Velocity = new Vector2(0, Velocity.Y);
    }

    public void Push(float velocity) {
        Velocity += new Vector2(velocity, 0);
    }
}