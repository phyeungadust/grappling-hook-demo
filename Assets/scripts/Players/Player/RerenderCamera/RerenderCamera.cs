using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class RerenderCamera : UdonSharpBehaviour
{

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private CustomTrackedObject trackedObj; 

    public void CustomStart()
    {
        this.cam.enabled = true;
        this.trackedObj.CustomStart();
    }

    public void CustomUpdate()
    {
        this.trackedObj.CustomUpdate();
    }

}