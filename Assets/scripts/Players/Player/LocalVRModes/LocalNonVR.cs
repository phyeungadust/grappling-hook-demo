using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class LocalNonVR : LocalVRMode
{

    public override bool IsLocal() => true;
    public override bool IsVR() => false;

    public override void Accept(LocalVRVisitor visitor)
    {
        visitor.VisitLocalNonVR(this);
    }

}