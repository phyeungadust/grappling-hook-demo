using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class CustomTrackedObjectControls : CustomControls
{

    [SerializeField]
    private CustomTrackedObject trackedObject;

    public override void CustomStart()
    {
        this.trackedObject.CustomStart();
    }

    public override void CustomUpdate()
    {
        this.trackedObject.CustomUpdate();
    }

}