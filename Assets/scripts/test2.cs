using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class test2 : UdonSharpBehaviour
{

    private VRCPlayerApi localPlayer;

    void Start()
    {
        
        this.localPlayer = Networking.LocalPlayer;

    }

    void Update()
    {
        if (this.localPlayer != null)
        {
            VRCPlayerApi.TrackingData tt = this
                .localPlayer
                .GetTrackingData(VRCPlayerApi.TrackingDataType.LeftHand);
            this.transform.position = tt.position;
            this.transform.rotation = tt.rotation;
        }
    }

}
