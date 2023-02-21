using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TetherBrakeNonLocalHandleUpdateStrat : TetherStateOperationsStrat
    {

        [SerializeField]
        private TetherBrakeState state;

        public override void Exec(TetherController tetherController)
        {
            state.ropeVector = state.hit.point - tetherController.transform.position;
            state.normalizedRopeVector = state.ropeVector.normalized; // direction of acceleration
            state.ropeLength = state.ropeVector.magnitude;

            // this line is unrelated to any state
            // but is necessary for linerenderer to work
            tetherController.SetTetherLength(state.ropeLength);
        }

    }
}