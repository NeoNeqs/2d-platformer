using Godot;

namespace Platformer;

public partial class UI : Control {
    public override void _Ready() {
        SignalBus.Instance.CoinCollected += coins => {
            GetNode<Label>("HBoxContainer/CoinCount").Text = coins.ToString();
        };
    }
}