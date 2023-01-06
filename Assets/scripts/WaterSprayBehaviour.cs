using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class WaterSprayBehaviour : UdonSharpBehaviour
{

    private ParticleSystem waterSprayParticleSystem;
    private VRCPlayerApi localPlayer;
    void Start()
    {
        this.waterSprayParticleSystem = this
            .GetComponent<ParticleSystem>();
        this.localPlayer = Networking.LocalPlayer;
    }

    void Update()
    {

        // shoot from player's head
        // will modify to shooting from gun barrel
        VRCPlayerApi.TrackingData td = this
            .localPlayer
            .GetTrackingData(
                VRCPlayerApi.TrackingDataType.LeftHand
            );
        this.transform.SetPositionAndRotation(
            td.position,
            td.rotation
        );

        if (Input.GetKeyDown(KeyCode.T))
        {
            this.waterSprayParticleSystem.Play();
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            this.waterSprayParticleSystem.Stop();
        }

    }

}