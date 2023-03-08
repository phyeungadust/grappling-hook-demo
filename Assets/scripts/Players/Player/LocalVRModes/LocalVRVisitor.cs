using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class LocalVRVisitor : UdonSharpBehaviour
{
    public virtual void VisitLocalVR(LocalVR localVR) {}
    public virtual void VisitLocalNonVR(LocalNonVR localNonVR) {}
    public virtual void VisitNonLocalVR(NonLocalVR nonLocalVR) {}
    public virtual void VisitNonLocalNonVR(NonLocalNonVR nonLocalNonVR) {}
}