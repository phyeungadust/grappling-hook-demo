using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class ShellShootBehaviour : UdonSharpBehaviour
{

    [SerializeField]
    private VRCObjectPool shellPool;
    [SerializeField]
    private ShellShootProperties shellShootProperties;
    private int ownerID;
    private VRCPlayerApi localPlayer;

    void Start()
    {
        this.localPlayer = Networking.LocalPlayer;
        this.ownerID = this.shellShootProperties.OwnerID;
        if (this.localPlayer.playerId == this.ownerID)
        {
            // set this object and shellPool to the owner
            Networking.SetOwner(
                this.localPlayer,
                this.gameObject
            );
            Networking.SetOwner(
                this.localPlayer,
                this.shellPool.gameObject
            );
        }
    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            this.Start();
        }
    }

    void Update()
    {

        if (this.localPlayer.playerId == this.ownerID)
        {

            bool pressedUseKey = false;
            if (this.localPlayer.IsUserInVR())
            {
                pressedUseKey = Input.GetAxis("Oculus_CrossPlatform_PrimaryIndexTrigger") > 0;
            }
            else
            {
                pressedUseKey = Input.GetKeyDown(KeyCode.E);
            }
            if (pressedUseKey)
            {

                GameObject spawnedShell = this
                    .shellPool
                    .TryToSpawn();

                if (spawnedShell != null)
                {
                    Networking.SetOwner(
                        this.localPlayer,
                        spawnedShell
                    );
                }

            }

        }
    }

}