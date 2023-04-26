using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class LocalVR : LocalVRMode
{

    public override bool IsLocal() => true;
    public override bool IsVR() => true;

    public override void Accept(LocalVRVisitor visitor)
    {
        visitor.VisitLocalVR(this);
    }

}