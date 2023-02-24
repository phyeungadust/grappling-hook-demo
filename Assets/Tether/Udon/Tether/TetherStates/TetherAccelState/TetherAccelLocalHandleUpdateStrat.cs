using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TetherAccelLocalHandleUpdateStrat : TetherStateOperationsStrat
    {

        private TetherAccelState state;

        public TetherAccelLocalHandleUpdateStrat Init
        (
            TetherAccelState state
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
            // by default, maxSpeed is 25.0f
            Vector3 newPlayerVelocity = Vector3.ClampMagnitude(
                playerVelocity + acceleration * Time.deltaTime,
                tetherController.properties.maxSpeed
            );

            state.owner.SetVelocity(newPlayerVelocity);

        }

    }
}