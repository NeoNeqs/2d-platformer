using Godot;

namespace Platformer;

[GlobalClass]
public partial class FiniteStateMachine : Node {
    [Export] private State _currentState;

    public override void _Ready() {
        ChangeState(_currentState);
    }

    public void ChangeState<T>(T state = null) where T : State {
        _currentState?.ExitState();
        _currentState = state ?? GetNode<T>(typeof(T).Name);
        _currentState.EnterState();
    }
}