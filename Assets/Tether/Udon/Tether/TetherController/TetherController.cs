using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{

    /// <summary>
    /// Controller for tethering player to an object or tethering rigidbodies to the player.
    /// </summary>
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TetherController : UdonSharpBehaviour
    {

        [Header("Properties")]
        [Tooltip("A TetherProperties script that determines the physical properties of grappling. Can be shared, so that all grapples in a scene share the same properties.")]
        public TetherProperties properties;

        private bool editorMode = true;

        public PlayerStore ownerStore;
        public VRCPlayerApi owner;

        private float tetherInput = 0.0f;
        private bool tethering = false;
        private Vector3 tetherPoint = Vector3.zero;
        private GameObject tetherObject;
        private float tetherLength;

        public TetherStatesDict TetherStatesDict;
        private TetherState tetherState;

        [SerializeField]
        private TetherControllerNetworked controllerNetworked;

        public void CustomStart()
        {

            this.owner = this.ownerStore.playerApiSafe.Get();

            // initialize networked controller
            this.controllerNetworked.Init(this);

            // initially, not tethered to anything
            // TetherNoneState
            this.SwitchState(
                this
                    .TetherStatesDict
                    .TetherNoneState,
                true
            );

        }

        public TetherState SwitchState(TetherState tetherState, bool init = false)
        {

            // init arg marked true means this.tetherState is set for the 1st time
            // which means there isn't a previous state
            // which means no need to call Exit on the previous state
            // because there is no previous state to begin with

            if 
            (
                Networking.LocalPlayer.playerId == 2
                && this.owner.playerId == 1
            )
            {
                Debug.Log("prev tetherState: " + this.tetherState);
                Debug.Log("new thetherState: " + tetherState);
            }
            if (!init)
            {
                this.tetherState.Exit(this);
            }
            this.tetherState = tetherState;
            this.tetherState.Enter(this);
            return this.tetherState;

        }

        public void SwitchStateBroadcast(TetherState tetherState)
        {

            // this.SwitchState(tetherState);

            this.controllerNetworked.SwitchStateBroadcast(
                tetherState
            );
        }

        public void CustomUpdate()
        {
            this.tetherState.CheckStateChange(this);
        }

        public void CustomFixedUpdate()
        {
            Debug.Log("GetTetherInput(): " + this.GetTetherInput());
            this.tetherState.HandleUpdate(this);
        }

        public void OnDisable()
        {
            tethering = false;
        }

        public float GetTetherInput()
        {
            return tetherInput;
        }

        /// <summary>
        /// Gets input value as received from an input script.
        /// </summary>
        /// <returns>Input value as float with a deadzone applied.</returns>
        public float GetInput()
        {
            return tetherInput > properties.tetherInputDeadzone ? tetherInput : 0.0f;
        }

        /// <summary>
        /// Sets input value from external source.
        /// </summary>
        /// <param name="value">Float of input value</param>
        public void SetInput(float value)
        {
            tetherInput = value;
        }

        /// <summary>
        /// Gets state of tether.
        /// </summary>
        /// <returns>Boolean state if player is tethered to something.</returns>
        public bool GetTethering()
        {
            return tethering;
        }

        public void SetTethering(bool tethering)
        {
            this.tethering = tethering;
        }

        public void SetTetherObject(GameObject tetherObject)
        {
            this.tetherObject = tetherObject;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public GameObject GetTetherObject()
        {
            return tetherObject;
        }

        /// <summary>
        /// Gets starting point of tether.
        /// </summary>
        /// <returns>Vector3 of the starting point of the tether.</returns>
        public Vector3 GetTetherStartPoint()
        {
            return transform.position;
        }

        public void SetTetherPoint(Vector3 tetherPoint)
        {
            this.tetherPoint = tetherPoint;
        }

        /// <summary>
        /// Gets end point of tether.
        /// </summary>
        /// <returns>Vector3 of the end point the tether is connected to.</returns>
        public Vector3 GetTetherPoint()
        {
            return tetherObject.transform.TransformPoint(tetherPoint);
        }

    }
}