using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Player
{
    /// <summary>
    /// Script to track objects to the player's hands or head.
    /// </summary>
    public class TrackedObject : UdonSharpBehaviour
    {

        [Tooltip("Which GameObject to enable if vrEnabled matches which mode we are in.")]
        public GameObject vrEnabledObject;
        // [Tooltip("Which tracking point to attach this object to.")]
        public VRCPlayerApi.TrackingDataType trackingType;

        [SerializeField]
        private PlayerStore ownerStore;
        private VRCPlayerApi owner;

        public void CustomStart()
        {

            this.owner = this.ownerStore.playerApiSafe.Get();

            // if (this.owner.IsUserInVR())
            // {
            //     this.trackingType = VRCPlayerApi.TrackingDataType.RightHand;
            // }
            // else
            // {
            //     this.trackingType = VRCPlayerApi.TrackingDataType.Head;
            // }

            // localPlayer = Networking.LocalPlayer;

            // if (localPlayer != null)
            // {
            //     editorMode = false;

            //     vrEnabledObject.SetActive(vrEnabled == localPlayer.IsUserInVR());
            // }

            this.vrEnabledObject.SetActive(true);

        }

        public void CustomUpdate()
        {
            VRCPlayerApi.TrackingData data = this
                .owner
                .GetTrackingData(trackingType);
            transform.SetPositionAndRotation(data.position, data.rotation);
        }
    }
}