using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class CustomControlsManager : CustomControls
{

    [SerializeField]
    private CustomControls[] customControlsArr;

    public override void CustomStart()
    {
        foreach (CustomControls cc in this.customControlsArr)
        {
            cc.CustomStart();
        }
    }

    public override void CustomUpdate()
    {
        foreach (CustomControls cc in this.customControlsArr)
        {
            cc.CustomUpdate();
        }
    }

    public override void CustomLateUpdate()
    {
        foreach (CustomControls cc in this.customControlsArr)
        {
            cc.CustomLateUpdate();
        }
    }

    public override void CustomFixedUpdate()
    {
        foreach (CustomControls cc in this.customControlsArr)
        {
            cc.CustomFixedUpdate();
        }
    }

}