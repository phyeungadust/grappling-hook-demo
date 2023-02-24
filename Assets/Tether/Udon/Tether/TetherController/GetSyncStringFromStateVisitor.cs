using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class GetSyncStringFromStateVisitor : TetherStateVisitor
    {

        private TetherControllerNetworked controllerNetworked;
        [SerializeField]
        private GrapplableCollection grapplableCollection;

        public GetSyncStringFromStateVisitor Init
        (
            TetherControllerNetworked controllerNetworked
        )
        {
            this.controllerNetworked = controllerNetworked;
            return this;
        }

        public override void VisitTetherNoneState(TetherNoneState state)
        {
            this
                .controllerNetworked
                .SwitchStateBroadcastSyncString = string.Join(
                    " ",
                    System.Guid.NewGuid().ToString().Substring(0, 6),
                    nameof(TetherNoneState)
                );
        }

        public override void VisitTetherAccelState(TetherAccelState state)
        {
            this
                .controllerNetworked
                .SwitchStateBroadcastSyncString = string.Join(
                    " ",
                    System.Guid.NewGuid().ToString().Substring(0, 6),
                    nameof(TetherAccelState),
                    state.tetherObject.GetComponent<Grapplable>().id,
                    state.hitPoint.x,
                    state.hitPoint.y,
                    state.hitPoint.z
                );
        }

        public override void VisitTetherBrakeState(TetherBrakeState state)
        {
            this
                .controllerNetworked
                .SwitchStateBroadcastSyncString = string.Join(
                    " ",
                    System.Guid.NewGuid().ToString().Substring(0, 6),
                    nameof(TetherBrakeState),
                    state.tetherObject.GetComponent<Grapplable>().id,
                    state.hitPoint.x,
                    state.hitPoint.y,
                    state.hitPoint.z
                );
        }

        public override void VisitStunnedState(StunnedState state)
        {
            this
                .controllerNetworked
                .SwitchStateBroadcastSyncString = string.Join(
                    " ",
                    System.Guid.NewGuid().ToString().Substring(0, 6),
                    nameof(StunnedState),
                    state.timer
                );
        }

    }
}