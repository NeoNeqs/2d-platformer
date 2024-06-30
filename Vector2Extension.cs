using Godot;

namespace Platformer;

public static class Vector2Extension {
    public static Vector2 Decay(this Vector2 a, Vector2 b, int decay, float dt) {
        var result = new Vector2();
        float scalar = Mathf.Exp(-decay * dt);

        result.X = b.X + (a.X - b.X) * scalar;
        result.Y = b.Y + (a.Y - b.Y) * scalar;
        
        return result;
    }
}