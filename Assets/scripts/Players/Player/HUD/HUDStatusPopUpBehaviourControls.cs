using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class HUDStatusPopUpBehaviourControls : CustomControls
{
    [SerializeField]
    private HUDStatusPopUpBehaviour popup;
    public override void CustomStart()
    {
        this.popup.CustomStart();
    }
    public override void CustomUpdate()
    {
        this.popup.CustomUpdate();
    }
}