using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{

    public class TetherAccelState : TetherState
    {
    
        [SerializeField]
        private TetherStatesDict tetherStatesDict;

        public RaycastHit hit;
        public Vector3 ropeVector;
        public Vector3 normalizedRopeVector;
        public float ropeLength;

        public VRCPlayerApi owner;
        public TetherStateOperationsStrat HandleUpdateStrat;
        [SerializeField]
        private TetherAccelSetHandleUpdateStratLocalVRVisitor setHandleUpdateStratLocalVRVisitor;
        private LocalVRMode localVRMode;

        public TetherAccelState Initialize(RaycastHit hit)
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
                    // TetherBrakeState
                    tetherController.SwitchState(
                        this
                            .tetherStatesDict
                            .TetherBrakeState
                            .Initialize(this.hit)
                    );
                    return true;
                }
                else
                {
                    // tethered and not within brake length
                    // TetherAccelState (no state change)
                    return false;
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
            this.HandleUpdateStrat.Exec(tetherController);
        }

        public override void Enter(TetherController tetherController)
        {

            this.owner = tetherController.owner;
            this.localVRMode = tetherController.ownerStore.localVRMode;

            // sets HandleUpdate strategy based on localVRMode
            this.localVRMode.Accept(this.setHandleUpdateStratLocalVRVisitor);

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