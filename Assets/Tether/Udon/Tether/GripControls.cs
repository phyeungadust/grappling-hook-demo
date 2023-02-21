using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class GripControls : CustomControls
{
    [SerializeField]
    private Grip grip;
    public override void CustomStart()
    {
        this.grip.CustomStart();
    }
}