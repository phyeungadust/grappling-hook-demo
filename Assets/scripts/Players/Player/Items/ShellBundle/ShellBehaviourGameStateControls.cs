using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class ShellBehaviourGameStateControls : GameStateControls
{
    [SerializeField]
    private ShellBehaviour shell;
    public override void OnBeforeGameStarts()
    {
        this.shell.OnBeforeGameStarts();
    }
}