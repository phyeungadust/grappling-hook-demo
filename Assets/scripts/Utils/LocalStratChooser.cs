using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class LocalStratChooser : UdonSharpBehaviour
{
    public virtual void ChooseLocal() {}
    public virtual void ChooseNonLocal() {}
}