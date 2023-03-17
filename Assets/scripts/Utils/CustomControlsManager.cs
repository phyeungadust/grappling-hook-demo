using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class CustomControlsManager : CustomControls
{

    [SerializeField]
    protected CustomControls[] customControlsArr;
    // [SerializeField]
    // private PlayerStore ownerStore; // required iff localOnly == true
    // [SerializeField]
    // bool localOnly = false;

    public override void CustomStart()
    {

        // if (localOnly && !this.ownerStore.localVRMode.IsLocal())
        // {
        //     // if controlsManager runs only locally
        //     // but we checked that the owner isn't local
        //     // deactivate this gameObject and don't run any methods below
        //     this.gameObject.SetActive(false);
        //     return;
        // }

        foreach (CustomControls cc in this.customControlsArr)
        {
            if (cc.gameObject.activeInHierarchy)
            {
                cc.CustomStart();
            }
        }

    }

    public override void CustomUpdate()
    {
        foreach (CustomControls cc in this.customControlsArr)
        {
            if (cc.gameObject.activeInHierarchy)
            {
                cc.CustomUpdate();
            }
        }
    }

    public override void CustomLateUpdate()
    {
        foreach (CustomControls cc in this.customControlsArr)
        {
            if (cc.gameObject.activeInHierarchy)
            {
                cc.CustomLateUpdate();
            }
        }
    }

    public override void CustomFixedUpdate()
    {
        foreach (CustomControls cc in this.customControlsArr)
        {
            if (cc.gameObject.activeInHierarchy)
            {
                cc.CustomFixedUpdate();
            }
        }
    }

}