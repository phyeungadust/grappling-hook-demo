using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class NonLocalVR : LocalVRMode
{

    public override bool IsLocal() => false;
    public override bool IsVR() => true;

    public override void Accept(LocalVRVisitor visitor)
    {
        visitor.VisitNonLocalVR(this);
    }

}