﻿using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class ShellGrip : UdonSharpBehaviour
{

    [SerializeField]
    private Vector3 localVRGripPos;
    [SerializeField]
    private Vector3 localVRGripRot;
    [SerializeField]
    private Vector3 localNonVRGripPos;
    [SerializeField]
    private Vector3 localNonVRGripRot;
    [SerializeField]
    private Vector3 nonLocalVRGripPos;
    [SerializeField]
    private Vector3 nonLocalVRGripRot;
    [SerializeField]
    private Vector3 nonLocalNonVRGripPos;
    [SerializeField]
    private Vector3 nonLocalNonVRGripRot;

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
            }
            else
            {
                // localNonVR
                this.transform.localPosition = this.localNonVRGripPos;
                this.transform.localEulerAngles = this.localNonVRGripRot;
            }
        }
        else
        {
            if (this.localVRMode.IsVR())
            {
                // nonLocalVR
                this.transform.localPosition = this.nonLocalVRGripPos;
                this.transform.localEulerAngles = this.nonLocalVRGripRot;
            }
            else
            {
                // nonLocalNonVR
                this.transform.localPosition = this.nonLocalNonVRGripPos;
                this.transform.localEulerAngles = this.nonLocalNonVRGripRot;
            }
        }

    }

}