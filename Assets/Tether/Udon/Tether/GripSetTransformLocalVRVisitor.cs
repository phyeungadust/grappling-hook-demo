using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class GripSetTransformLocalVRVisitor : LocalVRVisitor
{

    [SerializeField]
    private Vector3 VRGripPos;
    [SerializeField]
    private Vector3 VRGripRot;
    [SerializeField]
    private Vector3 NonVRGripPos;
    [SerializeField]
    private Vector3 NonVRGripRot;

    public override void VisitLocalVR(LocalVR localVR)
    {
        this.VisitVR();
    }

    public override void VisitLocalNonVR(LocalNonVR localNonVR)
    {
        this.VisitNonVR();
    }

    public override void VisitNonLocalVR(NonLocalVR nonLocalVR)
    {
        this.VisitVR();
    }

    public override void VisitNonLocalNonVR(NonLocalNonVR nonLocalNonVR)
    {
        this.VisitNonVR();
    }

    public void VisitVR()
    {
        this.transform.localPosition = this.VRGripPos;
        this.transform.localEulerAngles = this.VRGripRot;
    }

    public void VisitNonVR()
    {
        this.transform.localPosition = this.NonVRGripPos;
        this.transform.localEulerAngles = this.NonVRGripRot;
    }

}