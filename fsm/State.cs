using System.Diagnostics;
using Godot;

namespace Platformer;

public abstract partial class State : Node {
    public override void _Ready() {
        Debug.Assert(GetParent() is FiniteStateMachine, "State must be direct child of FiniteStateMachine.");
        Debug.Assert(GetType().Name == Name, $"State \"{Name}\" name must equal to \"{GetType().Name}\".");
        SetActive(false);
    }

    public virtual void EnterState() {
        SetActive(true);
    }

    public virtual void ExitState() {
        SetActive(false);
    }

    public override void _PhysicsProcess(double delta) {
        if (!Engine.IsEditorHint()) return;
        
        SetProcess(false);
        SetPhysicsProcess(false);
    }
    
    protected FiniteStateMachine GetStateMachine() {
        return GetParent<FiniteStateMachine>();
    }

    private void SetActive(bool state) {
        SetProcess(state);
        SetPhysicsProcess(state);
        SetProcessInput(state);
        SetProcessShortcutInput(state);
        SetProcessUnhandledInput(state);
        SetProcessUnhandledKeyInput(state);
    }
}
