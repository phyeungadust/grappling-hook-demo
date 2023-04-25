using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TetherAccelLocalCheckStateChangeStrat : TetherStateOperationsStrat
    {

        private TetherAccelState state;

        public TetherAccelLocalCheckStateChangeStrat Init
        (
            TetherAccelState state
        )
        {
            this.state = state;
            return this;
        }

        public override void Exec(TetherController tetherController)
        {

            if (tetherController.GetTetherInput() > tetherController.properties.tetherInputDeadzone)
            {

                // player still holding grapple key

                if (
                    this.state.ropeLength > tetherController.properties.brakeLength
                    || tetherController.GetTetherObject().layer == 26
                    || tetherController.GetTetherObject().layer == 27
                )
                {
                    // one of below:
                    // 1: tethered and not within brake length; or
                    // 2: tethered to NonLocalHitbox
                    // 3: tethered to ItemPickUpBox
                    // TetherAccelState (no state change)
                }
                else
                {
                    // tethered; and
                    // within brake length; and
                    // tethered not to NonLocalHitbox; and
                    // tethered not to ItemPickUpBox
                    // TetherBrakeState
                    tetherController.SwitchStateBroadcast(
                        this
                            .state
                            .tetherStatesDict
                            .TetherBrakeState
                            .Init(
                                this.state.tetherObject,
                                this.state.hitPoint
                            )
                    );
                }

            }
            else
            {
                // not tethered
                // TetherNoneState
                tetherController.SwitchStateBroadcast(
                    this
                        .state
                        .tetherStatesDict
                        .TetherNoneState
                );
            }


        }

    }
}