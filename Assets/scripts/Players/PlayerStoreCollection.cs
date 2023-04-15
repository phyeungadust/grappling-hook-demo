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

    public CustomGameManager customGameManager;

    public override void OnPlayerJoined(VRCPlayerApi player)
    {

        int playerID = player.playerId;

        if (player.isLocal)
        {
            this.localPlayerStore = this.allPlayerStores[playerID];
        }

        ++this.playerCount;
        wsLogger.Log($"player {playerID} joined");
        wsLogger.Log($"playerCount: {this.playerCount}");
        this
            .allPlayerStores[playerID]
            .gameObject
            .SetActive(true);

        this.allPlayerStores[playerID].manager.CustomStart();

    }

    void Update()
    {
        for (int i = 1; i <= this.playerCount; ++i)
        {
            this.allPlayerStores[i].manager.CustomUpdate();
        }
    }

    void FixedUpdate()
    {
        for (int i = 1; i <= this.playerCount; ++i)
        {
            this.allPlayerStores[i].manager.CustomFixedUpdate();
        }
    }

    void LateUpdate()
    {
        for (int i = 1; i <= this.playerCount; ++i)
        {
            this.allPlayerStores[i].manager.CustomLateUpdate();
        }
    }

    public PlayerStore GetByID(int id) => this.allPlayerStores[id];
    public PlayerStore[] GetAll() => this.allPlayerStores;
    public int GetCount() => this.playerCount;
    public PlayerStore GetLocal() => this.localPlayerStore;

}