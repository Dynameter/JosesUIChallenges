using UnityEngine;
using System.Collections;

public sealed class WordTileState_StartGame : State
{
    public override void Enter(System.Collections.Generic.Dictionary<string, object> argArguments)
    {
        WordTileStateMachine.Instance.SM.SetCurrentStateTo<WordTileState_StartRound>();
    }
}
