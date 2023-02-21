using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TetherBrakeSetHandleUpdateStratLocalVRVisitor : LocalVRVisitor
    {

        [SerializeField]
        private TetherBrakeState state;
        [SerializeField]
        private TetherBrakeLocalHandleUpdateStrat localHandleUpdateStrat;
        [SerializeField]
        private TetherBrakeNonLocalHandleUpdateStrat nonLocalHandleUpdateStrat;

        public override void VisitLocalVR(LocalVR localVR)
        {
            this.VisitLocal();
        }

        public override void VisitLocalNonVR(LocalNonVR localNonVR)
        {
            this.VisitLocal();
        }

        public override void VisitNonLocalVR(NonLocalVR nonLocalVR)
        {
            this.VisitNonLocal();
        }

        public override void VisitNonLocalNonVR(NonLocalNonVR nonLocalNonVR)
        {
            this.VisitNonLocal();
        }

        public void VisitLocal()
        {
            this.state.HandleUpdateStrat = this.localHandleUpdateStrat;
        }

        public void VisitNonLocal()
        {
            this.state.HandleUpdateStrat = this.nonLocalHandleUpdateStrat;
        }

    }
}