using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Player
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TrackedObjectControls : CustomControls
    {

        [SerializeField]
        private TrackedObject trackedObject;

        public override void CustomStart()
        {
            this.trackedObject.CustomStart();
        }

        public override void CustomUpdate()
        {
            this.trackedObject.CustomUpdate();
        }

    }

}