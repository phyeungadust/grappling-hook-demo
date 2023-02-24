using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{
    /// <summary>
    /// Basic TetherController input example that reads an input directly. Combine with TrackedObject.
    /// </summary>
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TetherAxisInput : UdonSharpBehaviour
    {
        
        [Tooltip("TetherController script.")]
        public TetherController controller;

        [SerializeField]
        private PlayerStore ownerStore;
        private LocalVRMode localVRMode;

        [SerializeField]
        private TetherAxisInputReadTetherInputLocalVRVisitor readTetherInputLocalVRVisitor;

        public void CustomStart()
        {
            this.localVRMode = this.ownerStore.localVRMode;
        }

        public void CustomUpdate()
        {
            // read key input based on localVRMode
            this.localVRMode.Accept(
                this.readTetherInputLocalVRVisitor.Init(
                    this,
                    this.controller
                )
            );
        }

        private void OnDisable()
        {
            controller.SetInput(0.0f);
        }

    }
}