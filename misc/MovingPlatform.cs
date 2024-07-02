using Godot;

namespace Platformer;

public partial class MovingPlatform : Path2D {

    [Export] private float _speed;
    private PathFollow2D _pathFollow;
    private float _step;
    private int _dir = -1;

    public override void _Ready() {
        _pathFollow = GetNode<PathFollow2D>("_PathFollow2D");

        if (!Engine.IsEditorHint()) {
            var body = GetNode<AnimatableBody2D>("_AnimatableBody2D");
            foreach (Node child in GetChildren()) {
                if (child.Name.ToString().StartsWith('_') || child is not Node2D node2D) {
                    continue;
                }
                
                Vector2 pos = node2D.GlobalPosition;
                RemoveChild(node2D);
                body.AddChild(node2D);
                node2D.GlobalPosition = pos;
            }
        }
    }

    public override void _PhysicsProcess(double delta) {
        if (_step is < 0 or > 1) {
            _dir *= -1;
        }
        _step += 0.01f * _speed * (float)delta * _dir;
        _pathFollow.ProgressRatio = _step;
    }
}