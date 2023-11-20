using Godot;
using System;

public partial class Score : Resource
{
    [Signal]
    public delegate void ScoreChangedEventHandler(int newScore);

    [Export]
    public int Value { 
        get => _value;
        set {
            if(_value == value) return;

            _value = value;
            EmitSignal("ScoreChanged", _value);
        }
     }

    private int _value = 0;
}
