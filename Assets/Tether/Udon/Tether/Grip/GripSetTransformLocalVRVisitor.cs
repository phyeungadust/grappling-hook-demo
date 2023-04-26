using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class GripSetTransformLocalVRVisitor : LocalVRVisitor
{

    [SerializeField]
    private Vector3 LocalVRGripPos;
    [SerializeField]
    private Vector3 LocalVRGripRot;
    [SerializeField]
    private Vector3 NonLocalVRGripPos;
    [SerializeField]
    private Vector3 NonLocalVRGripRot;
    [SerializeField]
    private Vector3 NonVRGripPos;
    [SerializeField]
    private Vector3 NonVRGripRot;

    public override void VisitLocalVR(LocalVR localVR)
    {
        this.transform.localPosition = this.LocalVRGripPos;
        this.transform.localEulerAngles = this.LocalVRGripRot;
    }

    public override void VisitLocalNonVR(LocalNonVR localNonVR)
    {
        this.VisitNonVR();
    }

    public override void VisitNonLocalVR(NonLocalVR nonLocalVR)
    {
        this.transform.localPosition = this.NonLocalVRGripPos;
        this.transform.localEulerAngles = this.NonLocalVRGripRot;
    }

    public override void VisitNonLocalNonVR(NonLocalNonVR nonLocalNonVR)
    {
        this.VisitNonVR();
    }

    private void VisitNonVR()
    {
        this.transform.localPosition = this.NonVRGripPos;
        this.transform.localEulerAngles = this.NonVRGripRot;
    }

}