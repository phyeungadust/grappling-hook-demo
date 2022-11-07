using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{
    public class TetherStatesDict : UdonSharpBehaviour
    {
        public TetherNoneState tetherNoneState;
        public TetherAccelState tetherAccelState;
        public TetherBrakeState tetherBrakeState;
        void Start()
        {
            this.tetherNoneState = 
                this.transform.Find("TetherNoneState").GetComponent<TetherNoneState>();
            this.tetherAccelState = 
                this.transform.Find("TetherAccelState").GetComponent<TetherAccelState>();
            this.tetherBrakeState = 
                this.transform.Find("TetherBrakeState").GetComponent<TetherBrakeState>();
        }
    }
}