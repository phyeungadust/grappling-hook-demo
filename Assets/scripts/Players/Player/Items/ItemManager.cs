using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class ItemManager : UdonSharpBehaviour
{

    public ItemCollection itemCollection;

    private ItemControls[] itemControlsArr;
    private ItemControls nullItem;
    private ItemControls currentItem;

    [SerializeField]
    private CustomControls[] customControlsArr;

    [SerializeField]
    private PlayerStore ownerStore;
    private LocalVRMode localVRMode;

    // the ItemUpdate() method in an ItemControls
    // runs only when the item is equipped

    // the CustomUpdate() method in a customControls
    // runs regardless of what item is equipped
    // even when no item is equipped

    public void SwitchItem(ItemControls item)
    {
        this.SwitchItemBroadcast(item);
    }

    public void SwitchItemBroadcast(ItemControls item)
    {
        this.SwitchItemBroadcastSyncString = string.Join(
            " ",
            System.Guid.NewGuid().ToString().Substring(0, 6),
            item.GetItemID()
        );
    }

    [UdonSynced, FieldChangeCallback(nameof(SwitchItemBroadcastSyncString))]
    private string switchItemBroadcastSyncString;
    public string SwitchItemBroadcastSyncString
    {
        get => this.switchItemBroadcastSyncString;
        set
        {
            this.switchItemBroadcastSyncString = value;
            if (this.localVRMode.IsLocal())
            {
                this.RequestSerialization();
            }
            string[] args = value.Split(' ');
            string nonce = args[0];
            int itemID = System.Int32.Parse(args[1]);
            this.SwitchItemLocal(this.itemControlsArr[itemID]);
        }
    }

    private void SwitchItemLocal(ItemControls item)
    {
        this.currentItem.UnEquip();
        this.currentItem = item;
        this.currentItem.Equip();
    }

    public void EquipRandomItem()
    {
        // randomly select an item from item ID [1, itemLength)
        // item 0 is null item, not included in the random select
        this.SwitchItem(
            this.itemControlsArr[Random.Range(1, this.itemControlsArr.Length)]
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

        Networking.SetOwner(
            this.ownerStore.playerApiSafe.Get(),
            this.gameObject
        );

        this.localVRMode = this.ownerStore.localVRMode;

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