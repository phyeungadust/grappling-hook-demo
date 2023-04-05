using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class ShopMenuControllerControls : CustomControls
{
    [SerializeField]
    private ShopMenuController controller;
    public override void CustomStart()
    {
        this.controller.CustomStart();
    }
    public override void CustomUpdate()
    {
        this.controller.CustomUpdate();
    }
}