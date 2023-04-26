using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class StunnedState : TetherState
    {

        [SerializeField]
        private TetherStatesDict tetherStatesDict;
        public float timer;

        public StunnedState Initialize(float timer)
        {
            this.timer = timer;
            return this;
        }

        public override void Enter(TetherController tetherController)
        {

            LocalVRMode localVRMode = tetherController.ownerStore.localVRMode;

            if (localVRMode.IsLocal())
            {
                // stun player
                tetherController
                    .ownerStore
                    .playerApiSafe
                    .Get()
                    .Immobilize(true);
            }

            Debug.Log("stunned");

            // line below is unrelated to our states
            // but is necessary for linerenderer to work
            tetherController.SetTethering(false);

        }

        public override void Exit(TetherController tetherController)
        {

            LocalVRMode localVRMode = tetherController.ownerStore.localVRMode;

            if (localVRMode.IsLocal())
            {
                // unstun player
                tetherController
                    .ownerStore
                    .playerApiSafe
                    .Get()
                    .Immobilize(false);
            }

            Debug.Log("unstunned");

        }

        public override bool CheckStateChange(TetherController tetherController)
        {

            LocalVRMode localVRMode = tetherController.ownerStore.localVRMode;

            if (localVRMode.IsLocal())
            {

                if (this.timer <= 0)
                {
                    // stun timer ends
                    // TetherNoneState
                    tetherController.SwitchStateBroadcast(
                        this
                            .tetherStatesDict
                            .TetherNoneState
                    );
                    return true;
                }
                else
                {
                    // stun timer still ticking
                    // StunnedState (no state change)
                    return false;
                }
                
            }

            return false;

        }

        public override void HandleUpdate(TetherController tetherController)
        {

            LocalVRMode localVRMode = tetherController.ownerStore.localVRMode;

            if (localVRMode.IsLocal())
            {
                // timer countdown
                this.timer -= Time.fixedDeltaTime; 
            }

        }

        public override void Accept(TetherStateVisitor tetherStateVisitor)
        {
            tetherStateVisitor.VisitStunnedState(this);
        }

    }

}