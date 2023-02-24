using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TetherBrakeLocalCheckStateChangeStrat : TetherStateOperationsStrat
    {

        private TetherBrakeState state;

        public TetherBrakeLocalCheckStateChangeStrat Init
        (
            TetherBrakeState state
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
                    // TetherBrakeState (no state change)
                }
                else
                {
                    // tethered and not within brake length
                    // TetherAccelState
                    tetherController.SwitchStateBroadcast(
                        this
                            .state
                            .tetherStatesDict
                            .TetherAccelState
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