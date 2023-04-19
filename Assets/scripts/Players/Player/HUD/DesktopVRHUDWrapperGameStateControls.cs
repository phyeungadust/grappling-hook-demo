using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class DesktopVRHUDWrapperGameStateControls : GameStateControls
{
    [SerializeField]
    private DesktopVRHUDWrapper wrapper;
    public override void OnBeforeGameStarts()
    {
        this.wrapper.OnBeforeGameStarts();
    }
}