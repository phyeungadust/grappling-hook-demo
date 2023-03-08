using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class ItemControls : UdonSharpBehaviour
{
    public virtual void Init() {}
    public virtual void UnEquip() {}
    public virtual void Equip() {}
    public virtual void ItemUpdate() {}
    public virtual void ItemFixedUpdate() {}
}