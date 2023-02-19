using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class VRStratChooser : UdonSharpBehaviour
{
    public virtual void ChooseVR() {}
    public virtual void ChooseNonVR() {}
}