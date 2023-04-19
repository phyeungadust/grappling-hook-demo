using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class ItemManager : UdonSharpBehaviour
{

    public ItemCollection itemCollection;

    private Item currentItem;

    [SerializeField]
    private CustomControls[] customControlsArr;

    [SerializeField]
    private PlayerStore ownerStore;
    private LocalVRMode localVRMode;

    [SerializeField]
    private GameStateControls gameStateControls;

    [HideInInspector]
    public bool enabledItemUse = true;

    // the ItemUpdate() method in an ItemControls
    // runs only when the item is equipped

    // the CustomUpdate() method in a customControls
    // runs regardless of what item is equipped
    // even when no item is equipped

    public void SwitchItem(Item item)
    {
        this.SwitchItemBroadcast(item);
    }

    public void SwitchItemBroadcast(Item item)
    {
        this.SwitchItemBroadcastSyncString = string.Join(
            " ",
            System.Guid.NewGuid().ToString().Substring(0, 6),
            item.GetItemControls().GetItemID()
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
            // this.SwitchItemLocal(this.itemControlsArr[itemID]);
            this.SwitchItemLocal(this.itemCollection.GetById(itemID));
        }
    }

    private void SwitchItemLocal(Item item)
    {
        this.currentItem.GetItemControls().UnEquip();
        this.currentItem = item;
        this.currentItem.GetItemControls().Equip();
    }

    public void EquipRandomItem()
    {

        // randomly select an item from item ID [1, itemLength)
        // item 0 is null item, not included in the random select
        this.SwitchItem(
            this
                .itemCollection
                .GetById(Random.Range(1, this.itemCollection.GetLength()))
        );

    }

    public void EquipShellShoot()
    {
        this.SwitchItem(this.itemCollection.shellShoot);
    }

    public void EquipSprayGun()
    {
        this.SwitchItem(this.itemCollection.sprayGun);
    }

    public void EquipNullItem()
    {
        this.SwitchItem(this.itemCollection.nullItem);
    }

    public void OnBeforeGameStarts()
    {
        this.currentItem.GetGameStateControls().OnBeforeGameStarts();
    }

    public void CustomStart()
    {

        Networking.SetOwner(
            this.ownerStore.playerApiSafe.Get(),
            this.gameObject
        );

        this.localVRMode = this.ownerStore.localVRMode;

        this.itemCollection.Init();
        this.currentItem = this.itemCollection.nullItem;

        foreach (CustomControls customControls in this.customControlsArr)
        {
            customControls.CustomStart();
        }

        if (this.localVRMode.IsLocal())
        {
            // subscribe to game state changes
            this
                .ownerStore
                .playerStoreCollection
                .customGameManager
                .SubscribeToGameStateChanges(this.gameStateControls);
        }

        foreach (Item item in this.itemCollection.GetAll())
        {
            item.GetItemControls().Init();
        }

    }

    public void CustomUpdate()
    {
        foreach (CustomControls customControls in this.customControlsArr)
        {
            customControls.CustomUpdate();
        }
        this.currentItem.GetItemControls().ItemUpdate();
    }

    public void CustomFixedUpdate()
    {
        foreach (CustomControls customControls in this.customControlsArr)
        {
            customControls.CustomFixedUpdate();
        }
        this.currentItem.GetItemControls().ItemFixedUpdate();
    }

}