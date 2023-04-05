using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class AvatarSlot : UdonSharpBehaviour
{

    [SerializeField]
    private AvatarButtonController controller;
    public AvatarInfo avatarInfo;
    public AvatarSlotButton slotButton;

    public void OnSelect()
    {
        this.controller.Select(this);
    }

    public void OnDeselect()
    {
        this.slotButton.OnDeselect();
    }

}