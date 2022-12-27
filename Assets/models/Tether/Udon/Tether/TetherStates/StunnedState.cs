using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{
    public class StunnedState : TetherState
    {

        [SerializeField]
        private TetherStatesDict tetherStatesDict;
        private float timer;

        public StunnedState Initialize(float timer)
        {
            this.timer = timer;
            return this;
        }

        public override void Enter(TetherController tetherController)
        {

            // stun player
            Networking.LocalPlayer.Immobilize(true);

            Debug.Log("stunned");

            // line below is unrelated to our states
            // but is necessary for linerenderer to work
            tetherController.SetTethering(false);

        }

        public override void Exit(TetherController tetherController)
        {
            // unstun player
            Networking.LocalPlayer.Immobilize(false);
            Debug.Log("unstunned");
        }

        public override bool CheckStateChange(TetherController tetherController)
        {
            if (this.timer <= 0)
            {
                // stun timer ends
                // TetherNoneState
                tetherController.SwitchState(
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

        public override void HandleUpdate(TetherController tetherController)
        {
            // timer countdown
            this.timer -= Time.fixedDeltaTime; 
        }

    }

}