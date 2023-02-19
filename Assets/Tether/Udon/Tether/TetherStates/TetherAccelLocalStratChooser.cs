using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TetherAccelLocalStratChooser : LocalStratChooser
    {

        [SerializeField]
        private TetherAccelState state;
        [SerializeField]
        private TetherAccelLocalHandleUpdateStrat tetherAccelLocalHandleUpdateStrat;
        [SerializeField]
        private TetherAccelNonLocalHandleUpdateStrat tetherAccelNonLocalHandleUpdateStrat;

        public override void ChooseLocal()
        {
            this.state.HandleUpdateStrat = this.tetherAccelLocalHandleUpdateStrat;
        }

        public override void ChooseNonLocal()
        {
            this.state.HandleUpdateStrat = tetherAccelNonLocalHandleUpdateStrat;
        }

    }
}