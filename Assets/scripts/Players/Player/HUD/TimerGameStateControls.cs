using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class TimerGameStateControls : GameStateControls
{
    [SerializeField]
    private Timer timer;
    public override void OnBeforeGameStarts()
    {
        this.timer.OnBeforeGameStarts();
    }
    public override void OnAfterGameStarts()
    {
        this.timer.OnAfterGameStarts();
    }
    public override void OnBeforeGameEnds()
    {
        this.timer.OnBeforeGameEnds();
    }
    public override void OnAfterGameEnds()
    {
        this.timer.OnAfterGameEnds();
    }
}