using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class CustomControlsManager : CustomControls
{

    [SerializeField]
    private CustomControls[] tetherControls;

    public override void CustomStart()
    {
        foreach (CustomControls cc in tetherControls)
        {
            cc.CustomStart();
        }
    }

    public override void CustomUpdate()
    {
        foreach (CustomControls cc in tetherControls)
        {
            cc.CustomUpdate();
        }
    }

    public override void CustomLateUpdate()
    {
        foreach (CustomControls cc in tetherControls)
        {
            cc.CustomLateUpdate();
        }
    }

    public override void CustomFixedUpdate()
    {
        foreach (CustomControls cc in tetherControls)
        {
            cc.CustomFixedUpdate();
        }
    }

}