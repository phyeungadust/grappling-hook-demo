using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class PlayerCameraFollower : UdonSharpBehaviour
{

    [SerializeField]
    private bool moveGameObjectToCam = false;

    [SerializeField]
    private VRCPlayerApiSafe vrcPlayerApiSafe;
    private VRCPlayerApi.TrackingData cachedTD;

    private bool initialized = false;

    void Update()
    {

        VRCPlayerApi player = this.vrcPlayerApiSafe.GetVRCPlayerApi();

        if (player == null)
        {
            this.cachedTD = new VRCPlayerApi.TrackingData(
                Vector3.zero,
                Quaternion.identity
            );
        }
        else
        {
            this.cachedTD = player.GetTrackingData(
                VRCPlayerApi.TrackingDataType.Head
            );
        }

        if (this.moveGameObjectToCam)
        {
            this.transform.SetPositionAndRotation(
                this.cachedTD.position,
                this.cachedTD.rotation
            );
        }

        this.initialized = true;

    }

    public VRCPlayerApi.TrackingData GetTrackingData()
    {
        if (!this.initialized)
        {
            this.Update();
        }
        return this.cachedTD;
    }

}