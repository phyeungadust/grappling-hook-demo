using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TetherControllerControls : CustomControls
    {

        [SerializeField]
        private TetherController tetherController;

        public override void CustomStart()
        {
            this.tetherController.CustomStart();
        }

        public override void CustomUpdate()
        {
            this.tetherController.CustomUpdate();
        }

        public override void CustomFixedUpdate()
        {
            this.tetherController.CustomFixedUpdate();
        }

    }

}