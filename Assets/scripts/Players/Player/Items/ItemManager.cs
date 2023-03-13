using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class ItemManager : UdonSharpBehaviour
{

    public ItemCollection itemCollection;

    private ItemControls[] itemControlsArr;
    private ItemControls nullItem;
    private ItemControls currentItem;

    [SerializeField]
    private CustomControls[] customControlsArr;

    // the ItemUpdate() method in an ItemControls
    // runs only when the item is equipped

    // the CustomUpdate() method in a customControls
    // runs regardless of what item is equipped
    // even when no item is equipped

    public void SwitchItem(ItemControls item)
    {
        this.currentItem.UnEquip();
        this.currentItem = item;
        this.currentItem.Equip();
    }

    public void EquipRandomItem()
    {
        this.SwitchItem(
            this.itemControlsArr[Random.Range(0, this.itemControlsArr.Length)]
        );
    }

    public void EquipShellShoot()
    {
        this.SwitchItem(this.itemCollection.shellShootBehaviourControls);
    }

    public void EquipSprayGun()
    {
        this.SwitchItem(this.itemCollection.waterSprayGunBehaviourControls);
    }

    public void EquipNullItem()
    {
        this.SwitchItem(this.itemCollection.nullItemControls);
    }

    public void CustomStart()
    {

        this.itemCollection.Init();
        this.itemControlsArr = this.itemCollection.itemControlsArr;
        this.nullItem = this.itemCollection.nullItemControls;
        this.currentItem = this.nullItem;

        foreach (CustomControls customControls in this.customControlsArr)
        {
            customControls.CustomStart();
        }
        foreach (ItemControls itemControls in this.itemControlsArr)
        {
            itemControls.Init();
        }

    }

    public void CustomUpdate()
    {
        foreach (CustomControls customControls in this.customControlsArr)
        {
            customControls.CustomUpdate();
        }
        this.currentItem.ItemUpdate();
    }

    public void CustomFixedUpdate()
    {
        foreach (CustomControls customControls in this.customControlsArr)
        {
            customControls.CustomFixedUpdate();
        }
        this.currentItem.ItemFixedUpdate();
    }

}