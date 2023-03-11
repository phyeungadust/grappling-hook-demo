using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class RerenderCameraControls : CustomControls
{
    [SerializeField]
    private RerenderCamera rerenderCamera;
    public override void CustomStart()
    {
        this.rerenderCamera.CustomStart();
    }
    public override void CustomUpdate()
    {
        this.rerenderCamera.CustomUpdate();
    }
}