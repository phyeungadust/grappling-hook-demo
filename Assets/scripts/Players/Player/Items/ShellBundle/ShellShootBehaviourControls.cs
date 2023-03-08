using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class ShellShootBehaviourControls : ItemControls
{

    [SerializeField]
    private ShellShootBehaviour shellShootBehaviour;

    public override void Init()
    {
        this.shellShootBehaviour.Init();
    }

    public override void Equip()
    {
        this.shellShootBehaviour.Equip();
    }

    public override void UnEquip()
    {
        this.shellShootBehaviour.UnEquip();
    }

    public override void ItemUpdate()
    {
        this.shellShootBehaviour.ItemUpdate();
    }

}