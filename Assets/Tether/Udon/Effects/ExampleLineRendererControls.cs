using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Effects
{

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)] 
    public class ExampleLineRendererControls : CustomControls
    {

        [SerializeField]
        private ExampleLineRenderer exampleLineRenderer;

        public override void CustomLateUpdate()
        {
            this.exampleLineRenderer.CustomLateUpdate();
        }

    }

}