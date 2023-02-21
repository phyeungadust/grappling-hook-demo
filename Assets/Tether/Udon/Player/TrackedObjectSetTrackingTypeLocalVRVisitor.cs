using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Player
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TrackedObjectSetTrackingTypeLocalVRVisitor : LocalVRVisitor
    {

        [SerializeField]
        private TrackedObject trackedObject;

        public override void VisitLocalVR(LocalVR localVR)
        {
            this.VisitVR();
        }

        public override void VisitLocalNonVR(LocalNonVR localNonVR)
        {
            this.VisitNonVR();
        }

        public override void VisitNonLocalVR(NonLocalVR nonLocalVR)
        {
            this.VisitVR();
        }

        public override void VisitNonLocalNonVR(NonLocalNonVR nonLocalNonVR)
        {
            this.VisitNonVR();
        }

        public void VisitVR()
        {
            this.trackedObject.trackingType = VRCPlayerApi.TrackingDataType.RightHand;
        }

        public void VisitNonVR()
        {
            this.trackedObject.trackingType = VRCPlayerApi.TrackingDataType.Head;
        }

    }
}