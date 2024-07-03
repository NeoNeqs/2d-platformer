using Godot;

namespace Platformer;

public partial class Level : Node2D {
    [Export] private PackedScene _nextLevel;

    private bool _unlocked;

    public void AdvanceLevel() {
        if (_nextLevel is not null && _unlocked) {
            GetTree().CallDeferred("change_scene_to_packed", _nextLevel);
        }
    }

    public void Unlock() {
        _unlocked = true;
    }

    public void Reset() {
        GetTree().CallDeferred("reload_current_scene");
    }
}