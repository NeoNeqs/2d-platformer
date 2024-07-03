using Godot;

namespace Platformer;

public partial class SignalBus : Node {
    
    [Signal]
    public delegate void CoinCollectedEventHandler(ushort coins);

    public static SignalBus Instance { private set; get; }

    public override void _Ready() {
        Instance = this;
    }

    public void EmitCoinCollected(ushort coins) {
        EmitSignal(SignalName.CoinCollected, coins);
    }
}
