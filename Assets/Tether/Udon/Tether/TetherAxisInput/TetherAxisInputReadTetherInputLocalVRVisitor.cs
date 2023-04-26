using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TetherAxisInputReadTetherInputLocalVRVisitor : LocalVRVisitor
    {

        private TetherController controller;
        private TetherAxisInput axisInput;
        [SerializeField]
        private string desktopTetherAxis;
        [SerializeField]
        private string vrTetherAxis;

        public TetherAxisInputReadTetherInputLocalVRVisitor Init
        (
            TetherAxisInput axisInput,
            TetherController controller
        )
        {
            this.axisInput = axisInput;
            this.controller = controller;
            return this;
        }

        public override void VisitLocalVR(LocalVR localVR)
        {
            float input = Input.GetAxis(this.vrTetherAxis);
            this.controller.SetInput(input);
        }

        public override void VisitLocalNonVR(LocalNonVR localNonVR)
        {
            float input = Input.GetAxis(this.desktopTetherAxis);
            this.controller.SetInput(input);
        }

        // if NonLocal, no need to read input

    }
}