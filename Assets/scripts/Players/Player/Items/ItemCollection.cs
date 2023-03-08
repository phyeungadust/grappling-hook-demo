using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class ItemCollection : UdonSharpBehaviour
{

    public ShellShootBehaviour shellShoot;
    public WaterSprayBehaviour sprayGun;
    public ItemControls nullItemControls;
    [HideInInspector]
    public ShellShootBehaviourControls shellShootBehaviourControls;
    [HideInInspector]
    public ItemControls[] itemControlsArr;

    public void Init()
    {
        this.shellShootBehaviourControls = this
            .shellShoot
            .GetComponent<ShellShootBehaviourControls>();
        this.itemControlsArr = new ItemControls[] {
            this.shellShootBehaviourControls
        };
    }

}