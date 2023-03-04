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
    [SerializeField]
    private WorldSpaceLogger wsLogger;
    private PlayerStore localPlayerStore;

    

    public override void OnPlayerJoined(VRCPlayerApi player)
    {

        if (player.isLocal)
        {
            this.localPlayerStore = this.GetByID(player.playerId);
        }

        ++this.playerCount;
        wsLogger.Log($"player {player.playerId} joined");
        wsLogger.Log($"playerCount: {this.playerCount}");
        this
            .allPlayerStores[player.playerId]
            .gameObject
            .SetActive(true);

    }

    public PlayerStore GetByID(int id) => this.allPlayerStores[id];
    public PlayerStore[] GetAll() => this.allPlayerStores;
    public int GetCount() => this.playerCount;
    public PlayerStore GetLocal() => this.localPlayerStore;

}