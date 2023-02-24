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

                if (this.state.ropeLength <= tetherController.properties.brakeLength)
                {
                    // tethered and within brake length
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
                else
                {
                    // tethered and not within brake length
                    // TetherAccelState (no state change)
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