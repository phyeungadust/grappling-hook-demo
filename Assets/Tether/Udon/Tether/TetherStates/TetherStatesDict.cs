using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TetherStatesDict : UdonSharpBehaviour
    {
        public TetherNoneState TetherNoneState;
        public TetherAccelState TetherAccelState;
        public TetherBrakeState TetherBrakeState;
        public StunnedState StunnedState;
    }

}