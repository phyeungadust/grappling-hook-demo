using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class ItemControls : UdonSharpBehaviour
{
    private int itemID;
    public virtual void Init() {}
    public virtual void UnEquip() {}
    public virtual void Equip() {}
    public virtual void ItemUpdate() {}
    public virtual void ItemFixedUpdate() {}
    public virtual ItemControls SetItemID(int id)
    {
        this.itemID = id;
        return this;
    }
    public virtual int GetItemID() => this.itemID;
}