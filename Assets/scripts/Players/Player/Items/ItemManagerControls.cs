using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class ItemManagerControls : CustomControls
{

    [SerializeField]
    private ItemManager itemManager;

    public override void CustomStart()
    {
        this.itemManager.CustomStart();
    }

    public override void CustomUpdate()
    {
        this.itemManager.CustomUpdate();
    }

    public override void CustomFixedUpdate()
    {
        this.itemManager.CustomFixedUpdate();
    }

}