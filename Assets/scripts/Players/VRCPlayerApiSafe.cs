using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class VRCPlayerApiSafe : UdonSharpBehaviour
{

    private VRCPlayerApi player = null;
    public int playerID = -1;

    public bool initialized = false;

    void Start()
    {
        if (this.playerID != -1)
        {
            this.SetPlayerByID(this.playerID);
        }
    }

    public void SetPlayer(VRCPlayerApi player)
    {
        this.player = player;
        this.playerID = player.playerId;
        this.initialized = true;
    }

    public void SetPlayerByID(int id)
    {
        VRCPlayerApi player = null;
        while (player == null)
        {
            player = VRCPlayerApi.GetPlayerById(id);
        }
        this.SetPlayer(player);
        this.initialized = true;
    }

    public VRCPlayerApi GetVRCPlayerApi()
    {
        if (initialized)
        {
            return this.player;
        }
        else
        {
            if (this.playerID == -1)
            {
                return null;
            }
            else
            {
                this.SetPlayerByID(this.playerID);
                return this.player;
            }
        }
    }


}