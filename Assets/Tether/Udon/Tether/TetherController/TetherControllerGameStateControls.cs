using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Tether
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TetherControllerGameStateControls : GameStateControls
    {
        [SerializeField]
        private TetherController controller;
        public override void OnBeforeGameStarts()
        {
            this.controller.OnBeforeGameStarts();
        }
        public override void OnAfterGameStarts()
        {
            this.controller.OnAfterGameStarts();
        }
    }
}