using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class GripVRChooser : VRStratChooser
{

    [SerializeField]
    private Vector3 VRGripPos;
    [SerializeField]
    private Vector3 VRGripRot;
    [SerializeField]
    private Vector3 NonVRGripPos;
    [SerializeField]
    private Vector3 NonVRGripRot;

    public override void ChooseVR()
    {
        this.transform.localPosition = this.VRGripPos;
        this.transform.localEulerAngles = this.VRGripRot;
    }

    public override void ChooseNonVR()
    {
        this.transform.localPosition = this.NonVRGripPos;
        this.transform.localEulerAngles = this.NonVRGripRot;
    }

}