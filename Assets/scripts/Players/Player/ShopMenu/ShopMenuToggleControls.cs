using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class ShopMenuToggleControls : CustomControls
{
    [SerializeField]
    private ShopMenuToggle toggle;
    public override void CustomStart()
    {
        this.toggle.CustomStart();
    }
    public override void CustomUpdate()
    {
        this.toggle.CustomUpdate();
    }
}