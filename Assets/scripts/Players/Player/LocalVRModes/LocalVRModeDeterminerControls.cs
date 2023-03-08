using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class LocalVRModeDeterminerControls : CustomControls
{

    [SerializeField]
    private LocalVRModeDeterminer localVRModeDeterminer;

    public override void CustomStart()
    {
        this.localVRModeDeterminer.CustomStart();
    }

}