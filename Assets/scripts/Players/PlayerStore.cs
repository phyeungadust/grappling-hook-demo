using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class PlayerStore : UdonSharpBehaviour
{

    public VRCPlayerApiSafe playerApiSafe;
    public PlayerStoreCollection playerStoreCollection;
    public PlayerHitbox hitbox; 

}