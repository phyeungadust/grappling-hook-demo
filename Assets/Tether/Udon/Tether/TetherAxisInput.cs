using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{
    /// <summary>
    /// Basic TetherController input example that reads an input directly. Combine with TrackedObject.
    /// </summary>
    public class TetherAxisInput : UdonSharpBehaviour
    {
        
        [Tooltip("TetherController script.")]
        public TetherController controller;

        [SerializeField]
        private string desktopTetherAxis;
        [SerializeField]
        private string vrTetherAxis;
        private string tetherInput;

        [SerializeField]
        private PlayerStore ownerStore;
        private VRCPlayerApi owner;

        private bool enabled = true;

        public void CustomStart()
        {
            this.owner = this.ownerStore.playerApiSafe.Get();
            if (this.owner.IsUserInVR())
            {
                this.tetherInput = this.vrTetherAxis;
            }
            else
            {
                this.tetherInput = this.desktopTetherAxis;
            }
        }

        public void CustomUpdate()
        {
            if (enabled)
            {
                float input = Input.GetAxis(tetherInput);
                controller.SetInput(input);
            }
        }

        public void Disable()
        {
            this.enabled = false;
        }

        private void OnDisable()
        {
            controller.SetInput(0.0f);
        }
    }
}