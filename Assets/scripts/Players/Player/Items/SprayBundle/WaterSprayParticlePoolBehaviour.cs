using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
public class WaterSprayParticlePoolBehaviour : UdonSharpBehaviour
{

    void Update()
    {
        VRCPlayerApi owner = VRCPlayerApi
            .GetPlayerById(1);
        if (owner != null)
        {
            VRCPlayerApi.TrackingData td = owner
                .GetTrackingData(
                    VRCPlayerApi.TrackingDataType.Head
                );
            this.transform.SetPositionAndRotation(
                td.position,
                td.rotation
            );
        }
    }

}