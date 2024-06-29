using Godot;

namespace Platformer;

public partial class UI : Control {
    public override void _Ready() {
        SignalBus.Instance.CoinCollected += OnCoinCollected;
    }

    private void OnCoinCollected(ushort coins) {
        GetNode<Label>("HBoxContainer/CoinCount").Text = coins.ToString();
    }

    public override void _ExitTree() {
        SignalBus.Instance.CoinCollected -= OnCoinCollected;
    }
}