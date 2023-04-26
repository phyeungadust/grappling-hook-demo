using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)] 
    public class TetherAxisInputControls : CustomControls
    {

        [SerializeField]
        private TetherAxisInput tetherAxisInput;

        public override void CustomStart()
        {
            this.tetherAxisInput.CustomStart();
        }

        public override void CustomUpdate()
        {
            this.tetherAxisInput.CustomUpdate();
        }

    }

}
