using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class AvatarInfo : UdonSharpBehaviour
{

    public Sprite avatarPreviewImg;
    public int price;
    public string avatarName;
    public string avatarState = "notPurchased"; // states: "notPurchased", "purchased", "equipped"
    public VRC_AvatarPedestal pedestal;

}