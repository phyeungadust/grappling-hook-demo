using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class ShellShootBehaviourGameStateControls : GameStateControls
{
    [SerializeField]
    private ShellShootBehaviour shellShoot;
    public override void OnBeforeGameStarts()
    {
        this.shellShoot.OnBeforeGameStarts();
    }
}