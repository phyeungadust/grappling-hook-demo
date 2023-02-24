using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TetherBrakeLocalHandleUpdateStrat : TetherStateOperationsStrat
    {

        private TetherBrakeState state;

        public TetherBrakeLocalHandleUpdateStrat Init
        (
            TetherBrakeState state
        )
        {
            this.state = state;
            return this;
        }

        public override void Exec(TetherController tetherController)
        {

            state.ropeVector = tetherController.GetTetherPoint() - tetherController.transform.position;
            state.normalizedRopeVector = state.ropeVector.normalized; // direction of acceleration
            state.ropeLength = state.ropeVector.magnitude;

            Vector3 playerVelocity = state.owner.GetVelocity();

            // acceleration along the rope 
            // to give an effect of pulling towards grapple point
            // larger pullFactor means pulling more intensely and faster
            // by default, pullFactor is 25.0f
            Vector3 acceleration 
                = state.normalizedRopeVector * tetherController.properties.pullFactor;

            // new player velocity after accelerating 
            // for an infinitesimal amount of time (Time.deltaTime)
            // maxspeed is clamped proportional to current ropeLength
            // (such that the speed becomes lower and lower to imitate braking)
            Vector3 newPlayerVelocity = Vector3.ClampMagnitude(
                playerVelocity + acceleration * Time.deltaTime,
                state.initialBrakeSpeed / tetherController.properties.brakeLength * state.ropeLength
            );

            state.owner.SetVelocity(newPlayerVelocity);

        }

    }
}