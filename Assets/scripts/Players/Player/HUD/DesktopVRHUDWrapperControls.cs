using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class DesktopVRHUDWrapperControls : CustomControls
{
    [SerializeField]
    private DesktopVRHUDWrapper wrapper;
    public override void CustomStart()
    {
        this.wrapper.CustomStart();
    }
    // public override void CustomUpdate()
    // {
    //     this.wrapper.CustomUpdate();
    // }
}