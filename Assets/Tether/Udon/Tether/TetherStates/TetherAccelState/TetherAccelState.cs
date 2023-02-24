using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TetherAccelState : TetherState
    {
    
        public TetherStatesDict tetherStatesDict;

        public GameObject tetherObject;
        public Vector3 hitPoint;
        public Vector3 ropeVector;
        public Vector3 normalizedRopeVector;
        public float ropeLength;

        public VRCPlayerApi owner;
        public TetherStateOperationsStrat CheckStateChangeStrat;
        public TetherStateOperationsStrat HandleUpdateStrat;
        [SerializeField]
        private TetherAccelSetHandleUpdateStratLocalVRVisitor setHandleUpdateStratLocalVRVisitor;
        [SerializeField]
        private TetherAccelSetCheckStateChangeStratLocalVRVisitor setCheckStateChangeStratLocalVRVisitor;
        private LocalVRMode localVRMode;

        public TetherAccelState Init(GameObject tetherObject, Vector3 hitPoint)
        {
            this.tetherObject = tetherObject;
            this.hitPoint = hitPoint;
            return this;
        }

        public override bool CheckStateChange(TetherController tetherController)
        {
            this.CheckStateChangeStrat.Exec(tetherController);
            return false;
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
            this.localVRMode.Accept(
                this.setHandleUpdateStratLocalVRVisitor.Init(this)
            );

            // sets CheckStateChange strategy based on localVRMode
            this.localVRMode.Accept(
                this.setCheckStateChangeStratLocalVRVisitor.Init(this)
            );

            // lines below are unrelated to our states
            // but are necessary for linerenderer to work
            tetherController.SetTetherObject(this.tetherObject);
            tetherController.SetTetherPoint(tetherController.GetTetherObject().transform.InverseTransformPoint(hitPoint));
            tetherController.SetTethering(true);

        }

        public override void Exit(TetherController tetherController) {}

        public override void Accept(TetherStateVisitor tetherStateVisitor)
        {
            tetherStateVisitor.VisitTetherAccelState(this);
        }

    }

}