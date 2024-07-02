using Godot;

public partial class Level : Node2D {
    [Export] private PackedScene _nextLevel;
    
    public bool Unlocked;

    public void AdvanceLevel() {
        if (_nextLevel is not null && Unlocked) {
            GetTree().CallDeferred("change_scene_to_packed", _nextLevel);
        }
    }

    public void Reset() {
        GetTree().CallDeferred("reload_current_scene");
    }
}