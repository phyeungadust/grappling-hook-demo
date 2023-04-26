using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TetherNoneState : TetherState
    {

        public TetherStatesDict tetherStatesDict;

        private VRCPlayerApi owner;
        private LocalVRMode localVRMode;

        [HideInInspector]
        public TetherStateOperationsStrat CheckStateChangeStrat;
        [SerializeField]
        private TetherNoneSetCheckStateChangeStratLocalVRVisitor setCheckStateChangeStratLocalVRVisitor;

        public override bool CheckStateChange(TetherController tetherController)
        {
            this.CheckStateChangeStrat.Exec(tetherController);
            return false;
        }

        public override void HandleUpdate(TetherController tetherController) {}

        public override void Enter(TetherController tetherController)
        {

            this.localVRMode = tetherController.ownerStore.localVRMode;

            // sets CheckStateChange strategy based on localVRMode
            this.localVRMode.Accept(
                this.setCheckStateChangeStratLocalVRVisitor.Init(this)
            );

            // line below is unrelated to our states
            // but is necessary for linerenderer to work
            tetherController.SetTethering(false);

        }
        
        public override void Exit(TetherController tethercontroller) {}

        public override void Accept(TetherStateVisitor tetherStateVisitor)
        {
            tetherStateVisitor.VisitTetherNoneState(this);
        }

        // check if wall is grapplable, i.e. within maxTetherLength from player to wall
        public bool TetherCast(TetherController tetherController, out RaycastHit hit)
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