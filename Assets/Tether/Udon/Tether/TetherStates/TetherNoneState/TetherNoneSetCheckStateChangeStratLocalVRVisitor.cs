using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TetherNoneSetCheckStateChangeStratLocalVRVisitor : LocalVRVisitor
    {

        private TetherNoneState state;
        [SerializeField]
        private TetherNoneLocalCheckStateChangeStrat localCheckStateChangeStrat;
        [SerializeField]
        private TetherStateOperationsStrat nonLocalCheckStateChangeStrat;

        public TetherNoneSetCheckStateChangeStratLocalVRVisitor Init
        (
            TetherNoneState state
        )
        {
            this.state = state;
            this.localCheckStateChangeStrat.Init(this.state);
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
            this.state.CheckStateChangeStrat = this.localCheckStateChangeStrat;
        }

        private void VisitNonLocal()
        {
            this.state.CheckStateChangeStrat = this.nonLocalCheckStateChangeStrat;
        }

    }
}