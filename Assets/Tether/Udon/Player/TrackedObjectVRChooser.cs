using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Player
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TrackedObjectVRChooser : VRStratChooser
    {

        [SerializeField]
        private TrackedObject trackedObject;

        public override void ChooseVR()
        {
            this.trackedObject.trackingType = VRCPlayerApi.TrackingDataType.RightHand;
        }

        public override void ChooseNonVR()
        {
            this.trackedObject.trackingType = VRCPlayerApi.TrackingDataType.Head;
        }

    }
}