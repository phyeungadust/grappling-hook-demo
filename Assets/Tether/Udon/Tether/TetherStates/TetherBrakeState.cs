using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{

    public class TetherBrakeState : TetherState
    {
    
        [SerializeField]
        private TetherStatesDict tetherStatesDict;

        private RaycastHit hit;
        private Vector3 ropeVector;
        private Vector3 normalizedRopeVector;
        private float ropeLength;
        private float initialBrakeSpeed; // initial player's vel. when braking state is entered
        private VRCPlayerApi localPlayer;
        
        public TetherBrakeState Initialize(RaycastHit hit)
        {
            this.hit = hit;
            return this;
        }

        public override bool CheckStateChange(TetherController tetherController)
        {
            
            if (tetherController.GetTetherInput() > tetherController.properties.tetherInputDeadzone)
            {

                // player still holding grapple key

                if (this.ropeLength <= tetherController.properties.brakeLength)
                {
                    // tethered and within brake length
                    // TetherBrakeState (no state change)
                    return false;
                }
                else
                {
                    // tethered and not within brake length
                    // TetherAccelState
                    tetherController.SwitchState(
                        this
                            .tetherStatesDict
                            .TetherAccelState
                            .Initialize(this.hit)
                    );
                    return true;
                }

            }

            // not tethered
            // TetherNoneState
            tetherController.SwitchState(
                this
                    .tetherStatesDict
                    .TetherNoneState
            );
            return true;

        }

        public override void HandleUpdate(TetherController tetherController)
        {

            this.ropeVector = hit.point - tetherController.transform.position;
            this.normalizedRopeVector = this.ropeVector.normalized; // direction of acceleration
            this.ropeLength = this.ropeVector.magnitude;

            // this line is unrelated to any state
            // but is necessary for linerenderer to work
            tetherController.SetTetherLength(this.ropeLength);

            Vector3 playerVelocity = this.localPlayer.GetVelocity();

            // acceleration along the rope 
            // to give an effect of pulling towards grapple point
            // larger pullFactor means pulling more intensely and faster
            // by default, pullFactor is 25.0f
            Vector3 acceleration 
                = normalizedRopeVector * tetherController.properties.pullFactor;

            // new player velocity after accelerating 
            // for an infinitesimal amount of time (Time.deltaTime)
            // maxspeed is clamped proportional to current ropeLength
            // (such that the speed becomes lower and lower to imitate braking)
            Vector3 newPlayerVelocity = Vector3.ClampMagnitude(
                playerVelocity + acceleration * Time.deltaTime,
                this.initialBrakeSpeed / tetherController.properties.brakeLength * this.ropeLength
            );

            this.localPlayer.SetVelocity(newPlayerVelocity);

        }

        public override void Enter(TetherController tetherController)
        {

            this.localPlayer = Networking.LocalPlayer;
            this.initialBrakeSpeed = this.localPlayer.GetVelocity().magnitude;

            // lines below are unrelated to our states
            // but are necessary for linerenderer to work
            tetherController.SetTetherObject(hit.collider.gameObject);
            tetherController.SetTetherPoint(tetherController.GetTetherObject().transform.InverseTransformPoint(hit.point));
            tetherController.SetTetherNormal(hit.normal);
            tetherController.SetTethering(true);

        }

        public override void Exit(TetherController tetherController) {}

    }

}