using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{

    public class TetherNoneState : TetherState
    {

        public GameObject tetherStates;

        public override TetherState HandleInput(TetherController tetherController)
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
                        return this
                            .tetherStates
                            .transform
                            .Find("TetherBrakeState")
                            .GetComponent<TetherBrakeState>()
                            .Initialize(hit);
                    }
                    else
                    {
                        // tethered and not within brake length
                        // TetherAccelState
                        return this
                            .tetherStates
                            .transform
                            .Find("TetherAccelState")
                            .GetComponent<TetherAccelState>()
                            .Initialize(hit);
                    }
                }

            }

            // not tethered
            // TetherNoneState (no state change)
            return this;

        }

        public override void HandleUpdate(TetherController tetherController) {}

        public override void Enter(TetherController tetherController)
        {
            // line below is unrelated to our states
            // but is necessary for linerenderer to work
            tetherController.SetTethering(false);
        }
        
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