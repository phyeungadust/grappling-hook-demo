using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TetherBrakeSetHandleUpdateStratLocalVRVisitor : LocalVRVisitor
    {

        private TetherBrakeState state;
        [SerializeField]
        private TetherBrakeLocalHandleUpdateStrat localHandleUpdateStrat;
        [SerializeField]
        private TetherStateOperationsStrat nonLocalHandleUpdateStrat;

        public TetherBrakeSetHandleUpdateStratLocalVRVisitor Init
        (
            TetherBrakeState state
        )
        {
            this.state = state;
            this.localHandleUpdateStrat.Init(this.state);
            return this;
        }

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

        private void VisitLocal()
        {
            this.state.HandleUpdateStrat = this.localHandleUpdateStrat;
        }

        private void VisitNonLocal()
        {
            this.state.HandleUpdateStrat = this.nonLocalHandleUpdateStrat;
        }

    }
}