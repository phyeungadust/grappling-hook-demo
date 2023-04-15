using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class ItemCollection : UdonSharpBehaviour
{

    private Item[] items;

    public Item[] GetAll() => this.items;
    public Item GetById(int id) => this.items[id];
    public int GetLength() => this.items.Length;

    public Item nullItem;
    public Item shellShoot;
    public Item sprayGun;

    public ItemControls nullItemControls;
    [HideInInspector]
    public ShellShootBehaviourControls shellShootBehaviourControls;
    [HideInInspector]
    public WaterSprayGunBehaviourControls waterSprayGunBehaviourControls;
    [HideInInspector]
    public ItemControls[] itemControlsArr;

    public void Init()
    {

        // this.shellShootBehaviourControls = this
        //     .shellShoot
        //     .GetComponent<ShellShootBehaviourControls>();
        // this.waterSprayGunBehaviourControls = this
        //     .sprayGun
        //     .GetComponent<WaterSprayGunBehaviourControls>();
        // this.itemControlsArr = new ItemControls[] {
        //     this.nullItemControls,
        //     this.shellShootBehaviourControls,
        //     this.waterSprayGunBehaviourControls
        // };

        this.items = new Item[] {
            this.nullItem,
            this.shellShoot,
            this.sprayGun
        };

        // assign every item its own ID
        for (int i = 0; i < this.items.Length; ++i)
        {
            this.items[i].GetItemControls().SetItemID(i);
        }

        // // assign every item its own ID
        // for (int i = 0; i < this.itemControlsArr.Length; ++i)
        // {
        //     this.itemControlsArr[i].SetItemID(i);
        // }

    }

}