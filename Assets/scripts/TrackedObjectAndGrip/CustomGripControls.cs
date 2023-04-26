using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class CustomGripControls : CustomControls
{

    [SerializeField]
    private CustomGrip grip;

    public override void CustomStart()
    {
        this.grip.CustomStart();
    }

}