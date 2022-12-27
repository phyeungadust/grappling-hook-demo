using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{

    public class TetherNoneState : TetherState
    {

        [SerializeField]
        private TetherStatesDict tetherStatesDict;

        public override bool CheckStateChange(TetherController tetherController)
        {
            
            if (tetherController.GetTetherInput() > tetherController.properties.tetherInputDeadzone)
            {

                // player hits grapple key

                bool detected = false;
                RaycastHit hit;

                detected = this.TetherCast(tetherController, out hit);

                if (detected)
                {
                    // grapple success
                    float ropeLength = Vector3.Distance(
                        tetherController.transform.position,
                        hit.point
                    );
                    if (ropeLength <= tetherController.properties.brakeLength)
                    {
                        // tethered and within brake length
                        // TetherBrakeState
                        tetherController.SwitchState(
                            this
                                .tetherStatesDict
                                .TetherBrakeState
                                .Initialize(hit)
                        );
                        return true;
                    }
                    else
                    {
                        // tethered and not within brake length
                        // TetherAccelState
                        tetherController.SwitchState(
                            this
                                .tetherStatesDict
                                .TetherAccelState
                                .Initialize(hit)
                        );
                        return true;
                    }
                }

            }

            // not tethered
            // TetherNoneState (no state change)
            return false;

        }

        public override void HandleUpdate(TetherController tetherController) {}

        public override void Enter(TetherController tetherController)
        {
            // line below is unrelated to our states
            // but is necessary for linerenderer to work
            tetherController.SetTethering(false);
        }
        
        public override void Exit(TetherController tethercontroller) {}

        // check if wall is grapplable, i.e. within maxTetherLength from player to wall
        private bool TetherCast(TetherController tetherController, out RaycastHit hit)
        {

            bool detected = false;

            // auto aim in incremental sizes
            detected = Physics.Raycast(
                tetherController.transform.position, 
                tetherController.transform.forward, 
                out hit, 
                tetherController.properties.tetherMaximumLength,
                tetherController.properties.tetherDetectionMask
            );

            if (!detected)
            {
                for (int i = tetherController.properties.tetherDetectionIncrements; detected == false && i > 0; i--)
                {
                    detected = Physics.SphereCast(
                        tetherController.transform.position, 
                        tetherController.properties.tetherDetectionSize / i, 
                        tetherController.transform.forward, 
                        out hit, 
                        tetherController.properties.tetherMaximumLength, 
                        tetherController.properties.tetherDetectionMask
                    );
                }
            }

            return detected;

        }

    }

}