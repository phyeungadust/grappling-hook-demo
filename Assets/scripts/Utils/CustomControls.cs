using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class CustomControls : UdonSharpBehaviour
{
    public virtual void CustomStart() {}
    public virtual void CustomUpdate() {}
    public virtual void CustomLateUpdate() {}
    public virtual void CustomFixedUpdate() {}
}