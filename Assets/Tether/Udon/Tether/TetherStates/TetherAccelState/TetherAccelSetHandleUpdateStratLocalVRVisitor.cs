using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TetherAccelSetHandleUpdateStratLocalVRVisitor : LocalVRVisitor
    {

        private TetherAccelState state;
        [SerializeField]
        private TetherAccelLocalHandleUpdateStrat localHandleUpdateStrat;
        [SerializeField]
        private TetherStateOperationsStrat nonLocalHandleUpdateStrat;

        public TetherAccelSetHandleUpdateStratLocalVRVisitor Init
        (
            TetherAccelState state
        )
        {
            this.state = state;
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
            this.state.HandleUpdateStrat = this
                .localHandleUpdateStrat
                .Init(this.state);
        }

        private void VisitNonLocal()
        {
            this.state.HandleUpdateStrat = this
                .nonLocalHandleUpdateStrat;
        }

    }
}