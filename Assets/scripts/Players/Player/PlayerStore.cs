using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class PlayerStore : UdonSharpBehaviour
{

    public VRCPlayerApiSafe playerApiSafe;
    [HideInInspector]
    public LocalVRMode localVRMode;
    public PlayerFollower follower;
    public PlayerHitbox hitbox; 
    public ItemManager itemManager;
    [HideInInspector]
    public PlayerStoreCollection playerStoreCollection;

}