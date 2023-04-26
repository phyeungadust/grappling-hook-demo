using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class WaterSprayGunBehaviourControls : ItemControls
{
    [SerializeField]
    private WaterSprayGunBehaviour sprayGun;
    public override void Init()
    {
        this.sprayGun.Init();
    }
    public override void Equip()
    {
        this.sprayGun.Equip();
    }
    public override void UnEquip()
    {
        this.sprayGun.UnEquip();
    }
    public override void ItemUpdate()
    {
        this.sprayGun.ItemUpdate();
    }
}