using UnityEngine;
using System.Collections;

public sealed class WordTileState_RunRound : State
{
    public override void Enter(System.Collections.Generic.Dictionary<string, object> argArguments)
    {
        base.Enter(argArguments);
        Debug.LogError("Running the thing");
    }
}
