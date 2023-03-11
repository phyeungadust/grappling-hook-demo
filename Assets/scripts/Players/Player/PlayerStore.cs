using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class PlayerStore : UdonSharpBehaviour
{

    public PlayerManager manager;
    public VRCPlayerApiSafe playerApiSafe;
    [HideInInspector]
    public LocalVRMode localVRMode;
    public PlayerFollower follower;
    public PlayerHitbox hitbox; 
    public ItemManager itemManager;
    public Tether.TetherController tetherController;
    public HUD hud;
    public InteractionMediator interactionMediator;
    [HideInInspector]
    public PlayerStoreCollection playerStoreCollection;

}