using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class NonLocalNonVR : LocalVRMode
{

    public override bool IsLocal() => false;
    public override bool IsVR() => false;

    public override void Accept(LocalVRVisitor visitor)
    {
        visitor.VisitNonLocalNonVR(this);
    }

}