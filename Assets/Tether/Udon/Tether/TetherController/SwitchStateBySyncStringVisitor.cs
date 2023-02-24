using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class SwitchStateBySyncStringVisitor : TetherStateVisitor
    {

        private TetherController controller;
        private TetherControllerNetworked controllerNetworked;
        [SerializeField]
        private GrapplableCollection grapplableCollection;

        public SwitchStateBySyncStringVisitor Init
        (
            TetherController controller,
            TetherControllerNetworked controllerNetworked
        )
        {
            this.controller = controller;
            this.controllerNetworked = controllerNetworked;
            return this;
        }

        public override void VisitTetherNoneState(TetherNoneState state)
        {
            this.controller.SwitchState(state);
        }

        public override void VisitTetherAccelState(TetherAccelState state)
        {

            string[] args = this
                .controllerNetworked
                .SwitchStateBroadcastSyncString
                .Split(' ');

            int tetherObjectGrapplableID = System.Int32.Parse(args[2]);
            float hitPointX = float.Parse(args[3]);
            float hitPointY = float.Parse(args[4]);
            float hitPointZ = float.Parse(args[5]);

            GameObject tetherObject = this
                .grapplableCollection
                .GetById(tetherObjectGrapplableID)
                .gameObject;

            Vector3 hitPoint = new Vector3(hitPointX, hitPointY, hitPointZ);

            this.controller.SwitchState(state.Init(
                tetherObject,
                hitPoint
            ));

        }

        public override void VisitTetherBrakeState(TetherBrakeState state)
        {

            string[] args = this
                .controllerNetworked
                .SwitchStateBroadcastSyncString
                .Split(' ');

            int tetherObjectGrapplableID = System.Int32.Parse(args[2]);
            float hitPointX = float.Parse(args[3]);
            float hitPointY = float.Parse(args[4]);
            float hitPointZ = float.Parse(args[5]);

            GameObject tetherObject = this
                .grapplableCollection
                .GetById(tetherObjectGrapplableID)
                .gameObject;

            Vector3 hitPoint = new Vector3(hitPointX, hitPointY, hitPointZ);

            this.controller.SwitchState(state.Init(
                tetherObject,
                hitPoint
            ));

        }

        public override void VisitStunnedState(StunnedState state)
        {

            string[] args = this
                .controllerNetworked
                .SwitchStateBroadcastSyncString
                .Split(' ');

            float stunTime = float.Parse(args[2]);

            this.controller.SwitchState(
                state.Initialize(stunTime)
            );

        }

    }
}