using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TetherStateVisitor : UdonSharpBehaviour
    {
        public virtual void VisitTetherNoneState(TetherNoneState state) {}
        public virtual void VisitTetherAccelState(TetherAccelState state) {}
        public virtual void VisitTetherBrakeState(TetherBrakeState state) {}
        public virtual void VisitStunnedState(StunnedState state) {}
    }
}