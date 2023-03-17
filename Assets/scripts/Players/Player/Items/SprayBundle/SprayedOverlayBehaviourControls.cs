using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class SprayedOverlayBehaviourControls : CustomControls
{
    [SerializeField]
    private SprayedOverlayBehaviour overlay;
    public override void CustomStart()
    {
        this.overlay.CustomStart();
    }
    public override void CustomUpdate()
    {
        this.overlay.CustomUpdate();
    }
    public override void CustomFixedUpdate()
    {
        this.overlay.CustomFixedUpdate();
    }
}