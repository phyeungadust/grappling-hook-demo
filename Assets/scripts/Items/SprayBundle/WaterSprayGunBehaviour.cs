using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class WaterSprayGunBehaviour : UdonSharpBehaviour
{

    [SerializeField]
    private int ownerID;
    [SerializeField]
    private VRCObjectPool waterSprayParticlePool;
    private VRCPlayerApi localPlayer;

    private bool currentlyHeld = false;

    void Start()
    {

        this.localPlayer = Networking.LocalPlayer;

        if (this.localPlayer.playerId == this.ownerID)
        {
            // set gun, particles, and particle pool to owner
            // specified by ownerID
            Networking.SetOwner(
                this.localPlayer,
                this.gameObject
            );
            Networking.SetOwner(
                this.localPlayer,
                this.waterSprayParticlePool.gameObject
            );
        }

    }

    // void FixedUpdate()
    // {

    //     // update gun position

    // }

    void Update()
    {

        VRCPlayerApi owner = VRCPlayerApi.GetPlayerById(this.ownerID);

        // if (owner != null)
        // {
        //     VRCPlayerApi.TrackingData td = owner
        //         .GetTrackingData(
        //             VRCPlayerApi.TrackingDataType.Head
        //         );
        //     this.transform.SetPositionAndRotation(
        //         td.position,
        //         td.rotation
        //     );
        // }

        // VRCPlayerApi owner = VRCPlayerApi
        //     .GetPlayerById(this.ownerID);
        // if (owner != null)
        // {
        // }

        if (this.localPlayer.playerId == this.ownerID)
        {
            if (currentlyHeld)
            {
                bool usePressed = false;
                if (this.localPlayer.IsUserInVR())
                {
                    usePressed = Input.GetAxis("Oculus_CrossPlatform_PrimaryIndexTrigger") > 0;
                }
                else
                {
                    usePressed = Input.GetKey(KeyCode.T);
                }
                if (usePressed)
                {
                    GameObject waterParticle = this
                        .waterSprayParticlePool
                        .TryToSpawn();
                    if (waterParticle != null)
                    {
                        Networking.SetOwner(
                            this.localPlayer,
                            waterParticle
                        );
                    }
                }
            }
        }

    }

    override public void OnPickup()
    {
        currentlyHeld = true;
    }

    override public void OnDrop()
    {
        currentlyHeld = false;
    }

}