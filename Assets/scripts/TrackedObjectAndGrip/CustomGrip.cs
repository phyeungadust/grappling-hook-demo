using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class CustomGrip : UdonSharpBehaviour
{

    [SerializeField]
    private Vector3 localVRGripPos;
    [SerializeField]
    private Vector3 localVRGripRot;
    [SerializeField]
    private Vector3 localVRScale = Vector3.one;
    [SerializeField]
    private Vector3 localNonVRGripPos;
    [SerializeField]
    private Vector3 localNonVRGripRot;
    [SerializeField]
    private Vector3 localNonVRScale = Vector3.one;
    [SerializeField]
    private Vector3 nonLocalVRGripPos;
    [SerializeField]
    private Vector3 nonLocalVRGripRot;
    [SerializeField]
    private Vector3 nonLocalVRScale = Vector3.one; 
    [SerializeField]
    private Vector3 nonLocalNonVRGripPos;
    [SerializeField]
    private Vector3 nonLocalNonVRGripRot;
    [SerializeField]
    private Vector3 nonLocalNonVRScale = Vector3.one;

    [SerializeField]
    private PlayerStore ownerStore;
    private LocalVRMode localVRMode;

    public void CustomStart()
    {

        this.localVRMode = this.ownerStore.localVRMode;

        if (this.localVRMode.IsLocal())
        {
            if (this.localVRMode.IsVR())
            {
                // localVR
                this.transform.localPosition = this.localVRGripPos;
                this.transform.localEulerAngles = this.localVRGripRot;
                this.transform.localScale = this.localVRScale;
            }
            else
            {
                // localNonVR
                this.transform.localPosition = this.localNonVRGripPos;
                this.transform.localEulerAngles = this.localNonVRGripRot;
                this.transform.localScale = this.localNonVRScale;
            }
        }
        else
        {
            if (this.localVRMode.IsVR())
            {
                // nonLocalVR
                this.transform.localPosition = this.nonLocalVRGripPos;
                this.transform.localEulerAngles = this.nonLocalVRGripRot;
                this.transform.localScale = this.nonLocalVRScale;
            }
            else
            {
                // nonLocalNonVR
                this.transform.localPosition = this.nonLocalNonVRGripPos;
                this.transform.localEulerAngles = this.nonLocalNonVRGripRot;
                this.transform.localScale = this.nonLocalNonVRScale;
            }
        }

    }

}