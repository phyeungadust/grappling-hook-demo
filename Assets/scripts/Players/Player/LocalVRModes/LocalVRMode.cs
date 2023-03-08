using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class LocalVRMode : UdonSharpBehaviour
{
    public virtual bool IsVR() => false;
    public virtual bool IsLocal() => false;
    public virtual void Accept(LocalVRVisitor visitor) {}
}