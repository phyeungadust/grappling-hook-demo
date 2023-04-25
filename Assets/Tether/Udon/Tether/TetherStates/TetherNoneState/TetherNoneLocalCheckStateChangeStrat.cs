using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TetherNoneLocalCheckStateChangeStrat : TetherStateOperationsStrat
    {

        private TetherNoneState state;

        public TetherNoneLocalCheckStateChangeStrat Init
        (
            TetherNoneState state
        )
        {
            this.state = state;
            return this;
        }

        public override void Exec(TetherController tetherController)
        {

            if (tetherController.GetTetherInput() > tetherController.properties.tetherInputDeadzone)
            {

                // player hits grapple key

                bool detected = false;
                RaycastHit hit;

                detected = this.state.TetherCast(tetherController, out hit);

                if (detected)
                {

                    // grapple success
                    float ropeLength = Vector3.Distance(
                        tetherController.transform.position,
                        hit.point
                    );

                    GameObject tetherObject = hit.collider.gameObject;
                    Vector3 hitPoint = hit.point;

                    if (
                        ropeLength > tetherController.properties.brakeLength
                        || tetherObject.layer == 26
                        || tetherObject.layer == 27
                    )
                    {
                        // either tethered out of brake length
                        // or tethered to a NonLocalHitbox
                        // or tethered to an ItemPickUpBox
                        tetherController.SwitchStateBroadcast(
                            this
                                .state
                                .tetherStatesDict
                                .TetherAccelState
                                .Init(tetherObject, hitPoint)
                        );
                    }
                    else
                    {
                        // tethered to a MapProps
                        // and is within brake length
                        tetherController.SwitchStateBroadcast(
                            this
                                .state
                                .tetherStatesDict
                                .TetherBrakeState
                                .Init(tetherObject, hitPoint)
                        );
                    }

                }

            }
            else
            {
                // not tethered
                // TetherNoneState (no state change)
            }

        }

    }
}