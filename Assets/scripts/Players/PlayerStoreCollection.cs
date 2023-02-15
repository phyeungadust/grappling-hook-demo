using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class PlayerStoreCollection : UdonSharpBehaviour
{

    [SerializeField]
    private PlayerStore[] allPlayerStores;
    private int playerCount = 0;

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        ++this.playerCount;
        this
            .allPlayerStores[this.playerCount]
            .gameObject
            .SetActive(true);
    }

    public PlayerStore GetByID(int id) => this.allPlayerStores[id];
    public PlayerStore[] GetAll() => this.allPlayerStores;
    public int GetCount() => this.playerCount;

}