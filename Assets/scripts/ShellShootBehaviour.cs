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

    void Update()
    {

        if (this.localPlayer.playerId == this.ownerID)
        {

            if (Input.GetKeyDown(KeyCode.E))
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