using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Player
{

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class KeyToggleControls : CustomControls
    {

        [SerializeField]
        private KeyToggle keyToggle;

        public override void CustomStart()
        {
            this.keyToggle.CustomStart();
        }

    }

}